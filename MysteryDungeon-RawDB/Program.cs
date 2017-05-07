using Microsoft.CSharp;
using MysteryDungeon_RawDB.Models.PSMD;
using SkyEditor.Core.IO;
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

            var pkms = new List<Pokemon>();
            var pkmNames = File.ReadAllLines("plist.txt");
            for (int i = 0; i < pkmNames.Length; i++)
            {
                Pokemon pkm = new Pokemon
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
                Console.WriteLine(String.Format("Error Compiling Template: ({0}, {1}) {2}",
                                              err.Line, err.Column, err.ErrorText));
            }
            else
            {
                // Load the assembly
                Assembly asm = Assembly.LoadFrom(outputAssemblyName);
                if (asm == null)
                {
                    Console.WriteLine("Error loading template assembly");
                }
                else
                {
                    // Get the template type
                    Type typ = asm.GetType("RazorOutput.Template");
                    if (typ == null)
                    {
                        Console.WriteLine("Could not find type RazorOutput.Template in assembly {0}", asm.FullName);
                    }
                    else
                    {
                        TemplateBase newTemplate = Activator.CreateInstance(typ) as TemplateBase;
                        if (newTemplate == null)
                        {
                            Console.WriteLine("Could not construct RazorOutput.Template or it does not inherit from TemplateBase");
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

        static void Main(string[] args)
        {
            // Cleanup old execution
            foreach (var item in Directory.GetFiles(".", "Temp_*.dll"))
            {
                File.Delete(item);
            }
                        
            var romPath = args[0]; // Directory, not ROM. Not yet.
            var outputPath = args[1];
            var extension = "php";

            // Extract ROM if needed
            var tempDir = "theROM";
            if (File.Exists(romPath))
            {
                // It's needed
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                RunProgram("3dstool.exe", $"-xtf 3ds \"{romPath}\" -0 Partition0.bin");
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");

                romPath = tempDir;
            }

            // Create output directory
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var data = LoadPsmdData(romPath).Result;

            // Generate HTML
            // - Pokemon
            BuildView("Views/Pokemon/Index.cshtml", Path.Combine(outputPath, "pokemon." + extension), data.Pokemon);
            foreach (var item in data.Pokemon)
            {
                BuildView("Views/Pokemon/Details.cshtml", Path.Combine(outputPath, "pokemon", item.ID.ToString() + "." + extension), new PokemonDetailsViewModel(item, data));
            }
            // - Moves
            BuildView("Views/Moves/Index.cshtml", Path.Combine(outputPath, "moves." + extension), data.Moves);
            foreach (var item in data.Moves)
            {
                BuildView("Views/Moves/Details.cshtml", Path.Combine(outputPath, "moves", item.ID.ToString() + "." + extension), new MoveDetailsViewModel(item, data));
            }
        }
    }
}
