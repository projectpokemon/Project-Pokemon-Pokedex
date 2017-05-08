using DotNet3dsToolkit;
using Microsoft.CSharp;
using MysteryDungeon_RawDB.Models.EOS;
using MysteryDungeon_RawDB.Models.PSMD;
using SkyEditor.Core.IO;
using SkyEditor.ROMEditor.MysteryDungeon.Explorers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace MysteryDungeon_RawDB
{
    class Program
    {

        public static void RunProgram(string program, string args)
        {
            var p = new Process();
            p.StartInfo.FileName = program;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(program);
            p.StartInfo.Arguments = args;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            p.WaitForExit();
        }

        public static async Task<EosDataCollection> LoadEosData(string rawFilesDir)
        {
            var data = new EosDataCollection();
            var provider = new PhysicalIOProvider();
            var languageFile = new LanguageString();
            await languageFile.OpenFile(Path.Combine(rawFilesDir, "data", "MESSAGE", "text_e.str"), provider);

            var monsterFile = new MonsterMDFile();
            await monsterFile.OpenFile(Path.Combine(rawFilesDir, "data", "BALANCE", "monster.md"), provider);

            var pkms = new List<Models.EOS.Pokemon>();
            foreach (MonsterMDEntry item in monsterFile.Entries)
            {
                var newEntry = new Models.EOS.Pokemon();
                newEntry.ID = item.EntityID;
                newEntry.Name = languageFile.GetPokemonName(newEntry.ID % 600);
                pkms.Add(newEntry);
            }
            data.Pokemon = pkms.OrderBy(x => x.ID).ToList();
            return data;
        }

        public static async Task<PsmdDataCollection> LoadPsmdData(string rawFilesDir)
        {
            var data = new PsmdDataCollection();

            // Load abilities
            var abilities = new List<Ability>();
            var abilityNames = File.ReadAllLines("alist.txt");
            for (int i = 0; i < abilityNames.Length; i++)
            {
                abilities.Add(new Ability
                {
                    ID = i,
                    Name = abilityNames[i]
                });
            }
            data.Abilities = abilities;

            // Load types
            var types = new List<PkmType>();
            var typeNames = File.ReadAllLines("tlist.txt");
            for (int i = 0; i < typeNames.Length; i++)
            {
                types.Add(new PkmType
                {
                    ID = i,
                    Name = typeNames[i]
                });
            }
            data.Types = types;


            // Load moves
            var wazaFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.WazaDataInfo();
            await wazaFile.OpenFile(Path.Combine(rawFilesDir, "RomFS", "pokemon", "waza_data_info.bin"), new PhysicalIOProvider());

            var actFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.ActDataInfo();
            await actFile.OpenFile(Path.Combine(rawFilesDir, "RomFS", "dungeon", "act_data_info.bin"), new PhysicalIOProvider());

            var actHitFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.ActHitCountTableDataInfo();
            await actHitFile.OpenFile(Path.Combine(rawFilesDir, "RomFS", "dungeon", "act_hit_count_table_data_info.bin"), new PhysicalIOProvider());

            var moves = new List<Move>();
            var moveNames = File.ReadAllLines("mlist.txt");
            for (int i = 0; i < moveNames.Length; i++)
            {
                var m = new Move();
                m.ID = i;
                if (string.IsNullOrEmpty(moveNames[i]))
                {
                    m.Name = "(Unknown Move)";
                }
                else
                {
                    m.Name = moveNames[i];
                }

                var actDataInfo = actFile.Entries[wazaFile.Entries[i].ActDataInfoIndex];

                m.EffectRate = actDataInfo.EffectRate;
                m.HPBellyChangeValue = actDataInfo.HPBellyChangeValue;
                m.TrapFlag = actDataInfo.TrapFlag;
                m.StatusChange = actDataInfo.StatusChange;
                m.StatChangeIndex = actDataInfo.StatChangeIndex;
                m.TypeChange = actDataInfo.TypeChange;
                m.TerrainChange = actDataInfo.TerrainChange;
                m.BaseAccuracy = actDataInfo.BaseAccuracy;
                m.MaxAccuracy = actDataInfo.MaxAccuracy;
                m.SizeTypeMove = actDataInfo.SizeTypeMove;
                m.TypeID = actDataInfo.TypeID;
                m.Attribute = actDataInfo.Attribute;
                m.BaseDamage = actDataInfo.BaseDamage;
                m.MaxDamage = actDataInfo.MaxDamage;
                m.BasePP = actDataInfo.BasePP;
                m.MaxPP = actDataInfo.MaxPP;
                m.CutsCorners = actDataInfo.CutsCorners;
                m.MoreTimeToAttack = actDataInfo.MoreTimeToAttack;
                m.TilesCount = actDataInfo.TilesCount;
                m.Range = actDataInfo.Range;
                m.Target = actDataInfo.Target;
                m.PiercingAttack = actDataInfo.PiercingAttack;
                m.SleepAttack = actDataInfo.SleepAttack;
                m.FaintAttack = actDataInfo.FaintAttack;
                m.NearbyDamage = actDataInfo.NearbyDamage;
                m.HitCounterIndex = actDataInfo.HitCounterIndex;
                moves.Add(m);
            }
            data.Moves = moves;


            // Load multihit
            var multi = new List<MoveMultiHit>();
            for (int i = 0; i < actHitFile.Entries.Count; i++)
            {
                var m = new MoveMultiHit();
                var currentEntry = actHitFile.Entries[i];
                m.HitChance2 = currentEntry.HitChance2;
                m.HitChance3 = currentEntry.HitChance3;
                m.HitChance4 = currentEntry.HitChance4;
                m.HitChance5 = currentEntry.HitChance5;
                m.HitCountMaximum = currentEntry.HitCountMaximum;
                m.HitCountMinimum = currentEntry.HitCountMinimum;
                m.ID = i;
                m.RepeatUntilMiss = currentEntry.RepeatUntilMiss;
                multi.Add(m);
            }
            data.MoveMultiHit = multi;


            // Load Pokemon
            var pokemonFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.PokemonDataInfo();
            await pokemonFile.OpenFile(Path.Combine(rawFilesDir, "RomFS", "pokemon", "pokemon_data_info.bin"), new PhysicalIOProvider());

            var pkms = new List<Models.PSMD.Pokemon>();
            var pkmNames = File.ReadAllLines("plist.txt");
            for (int i = 0; i < pkmNames.Length; i++)
            {
                var pkm = new Models.PSMD.Pokemon
                {
                    ID = i,
                    Name = pkmNames[i]
                };
                var dataInfo = pokemonFile.Entries[i];
                if (dataInfo != null)
                {
                    pkm.DexNumber = dataInfo.DexNumber;
                    pkm.Category = dataInfo.Category;
                    pkm.ListNumber1 = dataInfo.ListNumber1;
                    pkm.ListNumber2 = dataInfo.ListNumber2;
                    pkm.BaseHP = dataInfo.BaseHP;
                    pkm.BaseAttack = dataInfo.BaseAttack;
                    pkm.BaseSpAttack = dataInfo.BaseSpAttack;
                    pkm.BaseDefense = dataInfo.BaseDefense;
                    pkm.BaseSpDefense = dataInfo.BaseSpDefense;
                    pkm.BaseSpeed = dataInfo.BaseSpeed;
                    pkm.EvolvesFromEntry = dataInfo.EvolvesFromEntry;
                    pkm.ExpTableNumber = dataInfo.ExpTableNumber;
                    pkm.Ability1 = dataInfo.Ability1;
                    pkm.Ability2 = dataInfo.Ability2;
                    pkm.AbilityHidden = dataInfo.AbilityHidden;
                    pkm.Type1 = dataInfo.Type1;
                    pkm.Type2 = dataInfo.Type2;
                    pkm.IsMegaEvolution = dataInfo.IsMegaEvolution;
                    pkm.MinEvolveLevel = dataInfo.MinEvolveLevel;
                    pkms.Add(pkm);
                }
            }
            data.Pokemon = pkms;

            // Load levelup data
            var levels = new List<PokemonLevelUp>();
            for (int i = 0; i < pokemonFile.Entries.Count; i++)
            {
                var pkm = pokemonFile.Entries[i];
                for (int j = 0; j < pkm.MoveLevels.Length; j++)
                {
                    if (pkm.Moves[j] != 0)
                    {
                        levels.Add(new PokemonLevelUp
                        {
                            PokemonID = i,
                            MoveID = pkm.Moves[j],
                            Level = pkm.MoveLevels[j]
                        });
                    }
                }
            }
            data.PokemonLevelUp = levels;

            // Load experience data
            var expTables = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.Experience();
            await expTables.OpenFile(Path.Combine(rawFilesDir, "RomFS", "pokemon", "experience.bin"), new PhysicalIOProvider());
            var exps = new List<ExperienceLevel>();
            for (int expTableNum = 0; expTableNum < expTables.Entries.Count; expTableNum++)
            {
                for (int level = 0; level < expTables.Entries[expTableNum].Count; level++)
                {
                    var e = expTables.Entries[expTableNum][level];
                    exps.Add(new ExperienceLevel
                    {
                        ExperienceTableNumber = expTableNum,
                        Level = level,
                        Exp = e.Exp,
                        AddedAttack = e.AddedAttack,
                        AddedDefense = e.AddedDefense,
                        AddedHP = e.AddedHP,
                        AddedSpAttack = e.AddedSpAttack,
                        AddedSpDefense = e.AddedSpDefense,
                        AddedSpeed = e.AddedSpeed
                    });
                }
            }
            data.Experience = exps;

            return data;
        }

        static void BuildView(string viewPath, string outputPath, object model)
        {
            Console.WriteLine("Building " + outputPath);

            // Set up the hosting environment

            // a. Use the C# language (you could detect this based on the file extension if you want to)
            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());

            // b. Set the base class
            host.DefaultBaseClass = typeof(TemplateBase).FullName;

            // c. Set the output namespace and type name
            host.DefaultNamespace = "RazorOutput";
            host.DefaultClassName = "Template";

            // d. Add default imports
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Collections.Generic");
            //host.NamespaceImports.Add("MysteryDungeon_RawDB.Models");
            //host.NamespaceImports.Add("MysteryDungeon_RawDB.Models.EOS");
            host.NamespaceImports.Add("MysteryDungeon_RawDB.Models.PSMD");
            var engine = new RazorTemplateEngine(host);
            var codeProvider = new CSharpCodeProvider();

            // To-Do: generate using more templates
            GeneratorResults razorResult = null;
            using (TextReader rdr = new StringReader(File.ReadAllText(viewPath)))
            {
                razorResult = engine.GenerateCode(rdr);
            }

            using (StringWriter sw = new StringWriter())
            {
                codeProvider.GenerateCodeFromCompileUnit(razorResult.GeneratedCode, sw, new CodeGeneratorOptions());
                File.WriteAllText("current.cs", sw.ToString());
            }

            // Compile the generated code into an assembly
            string outputAssemblyName = String.Format("Temp_{0}.dll", Guid.NewGuid().ToString("N"));
            CompilerResults results = codeProvider.CompileAssemblyFromDom(
                new CompilerParameters(new string[] {
                    typeof(Program).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\")
                }, outputAssemblyName),
                razorResult.GeneratedCode);

            if (results.Errors.HasErrors)
            {
                CompilerError err = results.Errors
                                           .OfType<CompilerError>()
                                           .Where(ce => !ce.IsWarning)
                                           .First();
                throw new Exception(String.Format("Error Compiling Template: ({0}, {1}) {2}",
                                              err.Line, err.Column, err.ErrorText));
            }
            else
            {
                // Load the assembly
                Assembly asm = Assembly.LoadFrom(outputAssemblyName);
                if (asm == null)
                {
                    throw new Exception("Error loading template assembly");
                }
                else
                {
                    // Get the template type
                    Type typ = asm.GetType("RazorOutput.Template");
                    if (typ == null)
                    {
                        throw new Exception(string.Format("Could not find type RazorOutput.Template in assembly {0}", asm.FullName));
                    }
                    else
                    {
                        TemplateBase newTemplate = Activator.CreateInstance(typ) as TemplateBase;
                        if (newTemplate == null)
                        {
                            throw new Exception("Could not construct RazorOutput.Template or it does not inherit from TemplateBase");
                        }
                        else
                        {
                            // Run the compiled view
                            newTemplate.GetType().GetProperty("Model").SetValue(newTemplate, model);
                            newTemplate.Execute();

                            // Save the output
                            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                            }
                            File.WriteAllText(Path.Combine(outputPath), newTemplate.Buffer.ToString());
                        }
                    }
                }
            }
        }

        static async Task BuildEOS(string eosPath, string outputPath)
        {
            var provider = new PhysicalIOProvider();
            var romDir = "eos-rawfiles";
            using (var eosROM = new GenericNDSRom())
            {
                await eosROM.OpenFile(eosPath, provider);
                await eosROM.Unpack(romDir, provider);
            }

            var data = await LoadEosData(romDir);

            // Generate HTML
            BuildView("Views/EOS/Index.cshtml", Path.Combine(outputPath, "eos", "index.php"), null);
            // - Copy style
            File.Copy("Views/EOS/style.css", Path.Combine(outputPath, "eos", "style.css"), true);
            // - Pokemon
            BuildView("Views/EOS/Pokemon/Index.cshtml", Path.Combine(outputPath, "eos", "pokemon", "index.php"), data.Pokemon);
            //foreach (var item in data.Pokemon)
            //{
            //    BuildView("Views/EOS/Pokemon/Details.cshtml", Path.Combine(outputPath, "eos", "pokemon", item.ID.ToString() + ".php"), new PokemonDetailsViewModel(item, data));
            //}
            //// - Moves
            //BuildView("Views/EOS/Moves/Index.cshtml", Path.Combine(outputPath, "eos", "moves", "index.php"), data.Moves);
            //foreach (var item in data.Moves)
            //{
            //    BuildView("Views/EOS/Moves/Details.cshtml", Path.Combine(outputPath, "eos", "moves", item.ID.ToString() + ".php"), new MoveDetailsViewModel(item, data));
            //}
            //// - Abilities
            //BuildView("Views/EOS/Abilities/Index.cshtml", Path.Combine(outputPath, "eos", "abilities", "index.php"), data.Abilities);
            //foreach (var item in data.Abilities)
            //{
            //    BuildView("Views/EOS/Abilities/Details.cshtml", Path.Combine(outputPath, "eos", "abilities", item.ID.ToString() + ".php"), new AbilityDetailsViewModel(item, data));
            //}
            //// - Types
            //BuildView("Views/EOS/Types/Index.cshtml", Path.Combine(outputPath, "eos", "types", "index.php"), data.Types);
            //foreach (var item in data.Types)
            //{
            //    BuildView("Views/EOS/Types/Details.cshtml", Path.Combine(outputPath, "eos", "types", item.ID.ToString() + ".php"), new TypeDetailsViewModel(item, data));
            //}

            //// Add breadcrumb titles
            //File.WriteAllText(Path.Combine(outputPath, "eos", "__nav.php"), "Pokemon Super Mystery Dungeon");
            //File.WriteAllText(Path.Combine(outputPath, "eos", "pokemon", "__nav.php"), "Pokédex");
            //File.WriteAllText(Path.Combine(outputPath, "eos", "moves", "__nav.php"), "Movedex");
            //File.WriteAllText(Path.Combine(outputPath, "eos", "abilities", "__nav.php"), "Abilitydex");
            //File.WriteAllText(Path.Combine(outputPath, "eos", "types", "__nav.php"), "Typedex");

            //// Serialize raw data
            SkyEditor.Core.Utilities.Json.SerializeToFile(Path.Combine(outputPath, "eos", "data.json"), data, new PhysicalIOProvider());
        }

        static void BuildPSMD(string psmdPath, string outputPath)
        {
            // Extract ROM if needed
            var tempDir = "theROM";
            if (File.Exists(psmdPath))
            {
                // It's needed
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                RunProgram("3dstool.exe", $"-xtf 3ds \"{psmdPath}\" -0 Partition0.bin");
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");

                psmdPath = tempDir;
            }

            // Create output directory
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var data = LoadPsmdData(psmdPath).Result;

            // Generate HTML
            BuildView("Views/PSMD/Index.cshtml", Path.Combine(outputPath, "psmd", "index.php"), null);
            // - Copy style
            File.Copy("Views/PSMD/style.css", Path.Combine(outputPath, "psmd", "style.css"), true);
            // - Pokemon
            BuildView("Views/PSMD/Pokemon/Index.cshtml", Path.Combine(outputPath, "psmd", "pokemon", "index.php"), data.Pokemon);
            foreach (var item in data.Pokemon)
            {
                BuildView("Views/PSMD/Pokemon/Details.cshtml", Path.Combine(outputPath, "psmd", "pokemon", item.ID.ToString() + ".php"), new PokemonDetailsViewModel(item, data));
            }
            // - Moves
            BuildView("Views/PSMD/Moves/Index.cshtml", Path.Combine(outputPath, "psmd", "moves", "index.php"), data.Moves);
            foreach (var item in data.Moves)
            {
                BuildView("Views/PSMD/Moves/Details.cshtml", Path.Combine(outputPath, "psmd", "moves", item.ID.ToString() + ".php"), new MoveDetailsViewModel(item, data));
            }
            // - Abilities
            BuildView("Views/PSMD/Abilities/Index.cshtml", Path.Combine(outputPath, "psmd", "abilities", "index.php"), data.Abilities);
            foreach (var item in data.Abilities)
            {
                BuildView("Views/PSMD/Abilities/Details.cshtml", Path.Combine(outputPath, "psmd", "abilities", item.ID.ToString() + ".php"), new AbilityDetailsViewModel(item, data));
            }
            // - Types
            BuildView("Views/PSMD/Types/Index.cshtml", Path.Combine(outputPath, "psmd", "types", "index.php"), data.Types);
            foreach (var item in data.Types)
            {
                BuildView("Views/PSMD/Types/Details.cshtml", Path.Combine(outputPath, "psmd", "types", item.ID.ToString() + ".php"), new TypeDetailsViewModel(item, data));
            }

            // Add breadcrumb titles
            File.WriteAllText(Path.Combine(outputPath, "psmd", "__nav.php"), "Pokemon Super Mystery Dungeon");
            File.WriteAllText(Path.Combine(outputPath, "psmd", "pokemon", "__nav.php"), "Pokédex");
            File.WriteAllText(Path.Combine(outputPath, "psmd", "moves", "__nav.php"), "Movedex");
            File.WriteAllText(Path.Combine(outputPath, "psmd", "abilities", "__nav.php"), "Abilitydex");
            File.WriteAllText(Path.Combine(outputPath, "psmd", "types", "__nav.php"), "Typedex");

            // Serialize raw data
            SkyEditor.Core.Utilities.Json.SerializeToFile(Path.Combine(outputPath, "psmd", "data.json"), data, new PhysicalIOProvider());
        }

        static void Main(string[] args)
        {
            // Cleanup old execution
            foreach (var item in Directory.GetFiles(".", "Temp_*.dll"))
            {
                File.Delete(item);
            }

            var eosPath = args[0];
            var psmdPath = args[1];
            var outputPath = args[2];

            BuildEOS(eosPath, outputPath).Wait();
            BuildPSMD(psmdPath, outputPath);
        }
    }
}
