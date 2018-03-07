using IPS_Pages_Publisher.Interfaces;
using Newtonsoft.Json;
using ProjectPokemon.Pokedex.Models.PSMD;
using SkyEditor.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex
{
    partial class Program
    {
        public static async Task<PsmdDataCollection> LoadPsmdData(string rawFilesDir)
        {
            // Extract ROM if needed
            var tempDir = "theROM";
            if (File.Exists(rawFilesDir))
            {
                // It's needed
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                RunProgram("3dstool.exe", $"-xtf 3ds \"{rawFilesDir}\" -0 Partition0.bin");
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin --exefs ExeFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                RunProgram("3dstool.exe", $"-xutf exefs ExeFS.bin --exefs-dir=\"{tempDir}/ExeFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");
                File.Delete("ExeFS.bin");
                rawFilesDir = tempDir;
            }

            var data = new PsmdDataCollection();

            // Load abilities
            var abilities = new List<Ability>();
            var abilityNames = File.ReadAllLines("Resources/PSMD/alist.txt");
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
            var typeNames = File.ReadAllLines("Resources/PSMD/tlist.txt");
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
            var moveNames = File.ReadAllLines("Resources/PSMD/mlist.txt");
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
            var pkmNames = File.ReadAllLines("Resources/PSMD/plist.txt");
            for (int i = 0; i < pkmNames.Length; i++)
            {
                var pkm = new Pokemon
                {
                    ID = i,
                    Name = pkmNames[i],
                    Data = data
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

            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            return data;
        }

        static void BuildPSMD(PsmdDataCollection data, string outputFilename)
        {   
            var output = new List<Category>();

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = "Psmd-Pokemon";
            catPkm.Records = new List<Record>();
            foreach (var item in data.Pokemon)
            {
                catPkm.Records.Add(new Record
                {
                    Title = item.ID.ToString().PadLeft(3, '0') + " " + item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Pokemon.Details>(new PokemonDetailsViewModel(item, data)),
                    InternalName = $"psmd-pkm-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catPkm.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Pokemon.Index>(data.Pokemon),
                InternalName = "psmd-pkm-index"
            });
            output.Add(catPkm);

            // Moves
            var catMoves = new Category();
            catMoves.Name = "Psmd-Moves";
            catMoves.Records = new List<Record>();
            foreach (var item in data.Moves)
            {
                catMoves.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Moves.Details>(new MoveDetailsViewModel(item, data)),
                    InternalName = $"psmd-move-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catMoves.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Moves.Index>(data.Moves),
                InternalName = "psmd-move-index"
            });
            output.Add(catMoves);

            // Abilities
            var catAbilities = new Category();
            catAbilities.Name = "Psmd-Abilities";
            catAbilities.Records = new List<Record>();
            foreach (var item in data.Abilities)
            {
                catAbilities.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Abilities.Details>(new AbilityDetailsViewModel(item, data)),
                    InternalName = $"psmd-ability-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catAbilities.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Abilities.Index>(data.Abilities),
                InternalName = "psmd-ability-index"
            });
            output.Add(catAbilities);

            // Types
            var catTypes = new Category();
            catTypes.Name = "Psmd-Types";
            catTypes.Records = new List<Record>();
            foreach (var item in data.Types)
            {
                catTypes.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Types.Details>(new TypeDetailsViewModel(item, data)),
                    InternalName = $"psmd-type-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catTypes.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Types.Index>(data.Types),
                InternalName = "psmd-type-index"
            });
            output.Add(catTypes);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));
        }
    }
}
