using DotNet3dsToolkit;
using IPS_Pages_Publisher.Interfaces;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using pk3DS.Core;
using pk3DS.Core.Structures;
using pk3DS.Core.Structures.Gen6;
using pk3DS.Core.Structures.PersonalInfo;
using ProjectPokemon.Pokedex.Models.EOS;
using ProjectPokemon.Pokedex.Models.Gen7;
using ProjectPokemon.Pokedex.Models.PSMD;
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

namespace ProjectPokemon.Pokedex
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


            var moveFile = new waza_p();
            await moveFile.OpenFile(Path.Combine(rawFilesDir, "data", "BALANCE", "waza_p.bin"), provider);

            // Load Types
            var types = new List<Models.EOS.PkmType>();
            for (int i = 0; i < 19; i++)
            {
                var t = new Models.EOS.PkmType();
                t.ID = i;
                t.Name = languageFile.GetTypeName(i);
                t.Pokemon = new List<Models.EOS.PokemonReference>();
                t.Moves = new List<Models.EOS.MoveReference>();
                types.Add(t);
            }
            data.Types = types;

            // Read Move Data
            var moves = new List<Models.EOS.Move>();
            for (int i = 0; i < moveFile.Moves.Count; i++)
            {
                var m = new Models.EOS.Move { ID = i, Name = languageFile.GetMoveName(i), RawData = moveFile.Moves[i] };
                m.TypeID = moveFile.Moves[i].Type;
                m.TypeName = languageFile.GetTypeName(m.TypeID);
                m.Category = moveFile.Moves[i].Category.ToString();
                m.BasePower = moveFile.Moves[i].BasePower;
                m.BasePP = moveFile.Moves[i].BasePP;
                m.BaseAccuracy = moveFile.Moves[i].MoveAccuracy;
                m.PokemonLevelUp = new List<Models.EOS.LevelPokemonReference>();
                m.PokemonTM = new List<Models.EOS.PokemonReference>();
                m.PokemonEgg = new List<Models.EOS.PokemonReference>();
                moves.Add(m);

                // Add to types
                types[m.TypeID].Moves.Add(new Models.EOS.MoveReference { ID = m.ID, Name = m.Name });
            }
            data.Moves = moves;

            // Read Pokemon
            var monsterFile = new MonsterMDFile();
            await monsterFile.OpenFile(Path.Combine(rawFilesDir, "data", "BALANCE", "monster.md"), provider);
            var pkms = new List<Models.EOS.Pokemon>();
            for (int i = 0; i < 600; i += 1)
            {
                var maleEntry = monsterFile.Entries[i];
                var femaleEntry = monsterFile.Entries.Count > i + 600 ? monsterFile.Entries[i + 600] : null;

                var entry = new Models.EOS.Pokemon();
                entry.ID = i;// monsterFile.Entries[i].EntityID;
                entry.Name = languageFile.GetPokemonName(entry.ID % 600);
                entry.DexNumber = maleEntry.DexNumber;
                entry.EvolveFromID = maleEntry.EvolveFrom;
                entry.EvolveFromName = languageFile.GetPokemonName(entry.EvolveFromID);
                switch (maleEntry.EvolveMethod)
                {
                    case MonsterMDEntry.PokemonEvolutionMethod.None:
                        entry.EvolveCriteria = "N/A";
                        break;
                    case MonsterMDEntry.PokemonEvolutionMethod.Level:
                        entry.EvolveCriteria = "Level " + maleEntry.EvolveParam.ToString();
                        break;
                    case MonsterMDEntry.PokemonEvolutionMethod.IQ:
                        entry.EvolveCriteria = maleEntry.EvolveParam.ToString() + " IQ Points";
                        break;
                    case MonsterMDEntry.PokemonEvolutionMethod.Items:
                        entry.EvolveCriteria = "Item #" + maleEntry.EvolveParam.ToString();
                        switch (maleEntry.EvolveItem)
                        {
                            case MonsterMDEntry.EvolutionaryItem.None:
                                break;
                            case MonsterMDEntry.EvolutionaryItem.LinkCable:
                                entry.EvolveCriteria += " and Link Cable";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.Unknown2:
                                entry.EvolveCriteria += " and unknown (2)";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.Unknown3:
                                entry.EvolveCriteria += " and unknown (3)";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.Unknown4:
                                entry.EvolveCriteria += " and unknown (4)";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.SunRibbon:
                                entry.EvolveCriteria += " and Sun Ribbon";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.LunarRibbon:
                                entry.EvolveCriteria += " and Lunar Ribbon";
                                break;
                            case MonsterMDEntry.EvolutionaryItem.BeautyScarf:
                                entry.EvolveCriteria += " and Beauty Scarf";
                                break;
                        }
                        break;
                    case MonsterMDEntry.PokemonEvolutionMethod.LinkCable:
                        entry.EvolveCriteria = "Link Cable";
                        break;
                    default:
                        entry.EvolveCriteria = "Unknown (" + ((int)maleEntry.EvolveMethod).ToString() + ")";
                        break;
                }

                if (maleEntry != null)
                {
                    var e = new Models.EOS.Pokemon.GenderInfo();

                    e.BaseATK = maleEntry.BaseATK;
                    e.BaseDEF = maleEntry.BaseDEF;
                    e.BaseSPATK = maleEntry.BaseSPATK;
                    e.BaseSPDEF = maleEntry.BaseSPDEF;
                    e.BaseHP = maleEntry.BaseHP;

                    entry.Male = e;
                }

                if (femaleEntry != null)
                {
                    var e = new Models.EOS.Pokemon.GenderInfo();

                    e.BaseATK = maleEntry.BaseATK;
                    e.BaseDEF = maleEntry.BaseDEF;
                    e.BaseSPATK = maleEntry.BaseSPATK;
                    e.BaseSPDEF = maleEntry.BaseSPDEF;
                    e.BaseHP = maleEntry.BaseHP;

                    entry.Female = e;
                }

                // Moves
                if (moveFile.PokemonLearnsets.Count > i)
                {
                    var levelupMoves = new List<Tuple<int, Models.EOS.Move>>();
                    foreach (var item in moveFile.PokemonLearnsets[i].LevelUpMoves)
                    {
                        if (moveFile.Moves.Count > item.Item2)
                        {
                            // Add move to Pokemon
                            levelupMoves.Add(new Tuple<int, Models.EOS.Move>(item.Item1, new Models.EOS.Move { ID = item.Item2, Name = languageFile.GetMoveName(item.Item2), RawData = moveFile.Moves[item.Item2] }));

                            // Add Pokemon to move
                            var levelUp = moves[item.Item2].PokemonLevelUp.FirstOrDefault(x => x.ID == entry.ID);
                            if (levelUp == null)
                            {
                                moves[item.Item2].PokemonLevelUp.Add(new Models.EOS.LevelPokemonReference { ID = entry.ID, Name = entry.Name, Levels = new List<string> { item.Item1.ToString() } });
                            }
                            else
                            {
                                levelUp.Levels.Add(item.Item1.ToString());
                            }

                        }
                    }
                    entry.LevelupMoves = levelupMoves;

                    var tmMoves = new List<Models.EOS.Move>();
                    foreach (var item in moveFile.PokemonLearnsets[i].TMMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            tmMoves.Add(new Models.EOS.Move { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonTM.Add(new Models.EOS.PokemonReference { ID = entry.ID, Name = entry.Name });
                        }
                    }
                    entry.TMMoves = tmMoves;

                    var eggMoves = new List<Models.EOS.Move>();
                    foreach (var item in moveFile.PokemonLearnsets[i].EggMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            eggMoves.Add(new Models.EOS.Move { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonEgg.Add(new Models.EOS.PokemonReference { ID = entry.ID, Name = entry.Name });
                        }

                    }
                    entry.EggMoves = eggMoves;
                }
                else
                {
                    entry.LevelupMoves = new List<Tuple<int, Models.EOS.Move>>();
                    entry.TMMoves = new List<Models.EOS.Move>();
                    entry.EggMoves = new List<Models.EOS.Move>();
                }

                pkms.Add(entry);

                // Add to types
                types[maleEntry.MainType].Pokemon.Add(new Models.EOS.PokemonReference { ID = entry.ID, Name = entry.Name });
                if (maleEntry.MainType != maleEntry.AltType)
                {
                    types[maleEntry.AltType].Pokemon.Add(new Models.EOS.PokemonReference { ID = entry.ID, Name = entry.Name });
                }
            }
            data.Pokemon = pkms;

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
            var types = new List<Models.PSMD.PkmType>();
            var typeNames = File.ReadAllLines("tlist.txt");
            for (int i = 0; i < typeNames.Length; i++)
            {
                types.Add(new Models.PSMD.PkmType
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

            var moves = new List<Models.PSMD.Move>();
            var moveNames = File.ReadAllLines("mlist.txt");
            for (int i = 0; i < moveNames.Length; i++)
            {
                var m = new Models.PSMD.Move();
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

        private static readonly byte[] Signature = { 0x03, 0x40, 0x03, 0x41, 0x03, 0x42, 0x03, 0x43, 0x03 }; // tail end of item::ITEM_CheckBeads
        internal static void getTMHMList(string ExeFSPath, ref ushort[] TMs)
        {
            if (ExeFSPath == null) return;
            string[] files = Directory.GetFiles(ExeFSPath);
            if (!File.Exists(files[0]) || !Path.GetFileNameWithoutExtension(files[0]).Contains("code")) return;
            byte[] data = File.ReadAllBytes(files[0]);
            int dataoffset = Util.IndexOfBytes(data, Signature, 0x400000, 0) + Signature.Length;
            if (data.Length % 0x200 != 0) return;

            List<ushort> tms = new List<ushort>();

            for (int i = 0; i < 100; i++) // TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            TMs = tms.ToArray();
        }

        public static SMDataCollection LoadSunMoonData(string rawFilesDir)
        {
            var data = new SMDataCollection();
            var config = new GameConfig(GameVersion.SM);
            config.Initialize(Path.Combine(rawFilesDir, "RomFS"), Path.Combine(rawFilesDir, "ExeFS"), lang: 2); // Language index 2 is English

            // Load strings
            var items = config.getText(TextName.ItemNames);
            var moveNames = config.getText(TextName.MoveNames);
            var moveflavor = config.getText(TextName.MoveFlavor);
            var species = config.getText(TextName.SpeciesNames);
            var abilities = config.getText(TextName.AbilityNames);
            var forms = config.getText(TextName.Forms);
            var types = config.getText(TextName.Types);
            var EXPGroups = new string[] { "Medium-Fast", "Erratic", "Fluctuating", "Medium-Slow", "Fast", "Slow" };
            var eggGroups = new string[] { "---", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass", "Human-Like", "Water 3", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered" };
            var colors = new string[] { "Red", "Blue", "Yellow", "Green", "Black", "Brown", "Purple", "Gray", "White", "Pink" };
            var tutormoves = new ushort[] { 520, 519, 518, 338, 307, 308, 434, 620 };

            // Load TMs
            var TMs = new ushort[0];
            getTMHMList(Path.Combine(rawFilesDir, "ExeFS"), ref TMs);

            // Load Pokemon
            // - Load Level-up GARC
            var levelupGarcFiles = config.getGARCData("levelup").Files;
            int[] baseForms, formVal;
            string[][] AltForms = config.Personal.getFormList(species, config.MaxSpeciesID);
            string[] specieslist = config.Personal.getPersonalEntryList(AltForms, species, config.MaxSpeciesID, out baseForms, out formVal);

            // - Load Egg move GARC
            var eggmoveGarcFiles = config.getGARCData("eggmove").Files;

            // - Load Evolution GARC
            var evolutionGarcFiles = config.getGARCData("evolution").Files;
            string[] evolutionMethods =
            {
                "", // No param
                "Level Up with Friendship", // No param
                "Level Up at Morning with Friendship", // No param
                "Level Up at Night with Friendship", // No param
                "Level Up at level {0}", // Level
                "Trade", // No param
                "Trade with Held Item: {0}", // Item
                $"Trade for opposite {specieslist[588]}/{specieslist[616]}", // Shelmet&Karrablast  // No param
                "Used Item: {0}", // Item
                "Level Up (Attack > Defense) at level {0}", // Level
                "Level Up (Attack = Defense) at level {0}", // Level
                "Level Up (Attack < Defense) at level {0}", // Level
                "Level Up (Random < 5) at level {0}", // Level
                "Level Up (Random > 5) at level {0}", // Level
                $"Level Up ({specieslist[291]}) at level {0}", // Ninjask // Level
                $"Level Up ({specieslist[292]}) at level {0}", // Shedinja // Level
                "Level Up (Beauty): {0}", // Beauty
                "Used Item (Male): {0}", // Kirlia->Gallade // Item
                "Used Item (Female): {0}", // Snorunt->Froslass // Item
                "Level Up with Held Item (Day): {0}", // Item
                "Level Up with Held Item (Night): {0}", // Item
                "Level Up with Move: {0}", // Move
                "Level Up with Party: {0}", // Species
                "Level Up Male at level {0}", // Level
                "Level Up Female at level {0}", // Level
                "Level Up at Electric",
                "Level Up at Forest",
                "Level Up at Cold",

                "Level Up with 3DS Upside Down at level {0}", // Level
                "Level Up with 50 Affection and a move of type {0}", // Type
                $"{types[16]} Type in Party " + "at level {0}", // Level
                "Overworld Rain at level {0}", // Level
                "Level Up (@) at Morning at level {0}", // Level
                "Level Up (@) at Night at level {0}", // Level
                "Level Up Female (SetForm 1) at level {0}", // Level
                "UNUSED",
                "Level Up Any Time on Version {0}", // Version
                "Level Up Daytime on Version {0}", // Version
                "Level Up Nighttime on Version {0}", // Version
                "Level Up Summit at level {0}", // Level
            };
            ushort[] evolutionMethodCase =
            {
                0,0,0,0,1,0,2,0,2,1,1,1,1,1,1,1,5,2,2,2,2,3,4,1,1,0,0,0, // 27, Past Methods
                // New Methods
                1, // 28 - Dark Type Party
                6, // 29 - Affection + MoveType
                1, // 30 - UpsideDown 3DS
                1, // 31 - Overworld Rain
                1, // 32 - Level @ Day
                1, // 33 - Level @ Night
                1, // 34 - Gender Branch
                1, // 35 - UNUSED
                7, 7, 7, // Version Specific
                1,
            };

            // - Load Mega Evolution GARC
            var megaEvoGarcFiles = config.getGARCData("megaevo").Files;

            // - Consolidate data
            var pkms = new List<Models.Gen7.Pokemon>();
            foreach (var item in config.Personal.Table)
            {
                var pkm = new Models.Gen7.Pokemon();

                pkm.ID = pkms.Count;
                if (species.Length > pkm.ID)
                {
                    pkm.Name = species[pkm.ID];
                }
                else
                {
                    pkm.Name = "(Unnamed)";
                }

                // Stats
                pkm.BaseHP = item.HP;
                pkm.BaseAttack = item.ATK;
                pkm.BaseDefense = item.DEF;
                pkm.BaseSpeed = item.SPE;
                pkm.BaseSpAttack = item.SPA;
                pkm.BaseSpDefense = item.SPD;
                pkm.HPEVYield = item.EV_HP;
                pkm.AttackEVYield = item.EV_ATK;
                pkm.DefenseEVYield = item.EV_DEF;
                pkm.SpeedEVYield = item.EV_SPE;
                pkm.SpAttackEVYield = item.EV_SPA;
                pkm.SpDefenseEVYield = item.EV_SPD;

                // Types
                pkm.Type1 = new TypeReference { ID = item.Types[0], Name = types[item.Types[0]] };
                pkm.Type2 = new TypeReference { ID = item.Types[1], Name = types[item.Types[1]] };

                pkm.CatchRate = item.CatchRate;
                pkm.EvoStage = item.EvoStage;

                // Items
                pkm.HeldItem1 = new ItemReference { ID = item.Items[0], Name = items[item.Items[0]] };
                pkm.HeldItem2 = new ItemReference { ID = item.Items[1], Name = items[item.Items[1]] };
                pkm.HeldItem3 = new ItemReference { ID = item.Items[2], Name = items[item.Items[2]] };

                pkm.Gender = item.Gender;
                pkm.HatchCycles = item.HatchCycles;
                pkm.BaseFriendship = item.BaseFriendship;

                pkm.ExpGrowth = EXPGroups[item.EXPGrowth];

                pkm.EggGroup1 = eggGroups[item.EggGroups[0]];
                pkm.EggGroup2 = eggGroups[item.EggGroups[1]];

                pkm.Ability1 = new AbilityReference { ID = item.Abilities[0], Name = abilities[item.Abilities[0]] };
                pkm.Ability2 = new AbilityReference { ID = item.Abilities[1], Name = abilities[item.Abilities[1]] };
                pkm.AbilityHidden = new AbilityReference { ID = item.Abilities[2], Name = abilities[item.Abilities[2]] };

                pkm.FormeCount = item.FormeCount;
                pkm.FormeSprite = item.FormeSprite;

                pkm.Color = colors[item.Color & 0xF];

                pkm.BaseExp = item.BaseEXP;
                pkm.BST = item.BST;

                pkm.Height = ((decimal)item.Height / 100);
                pkm.Weight = ((decimal)item.Weight / 10);

                // Level-up
                var pkmLevelup = new List<Models.Gen7.LevelupMoveReference>();
                var currentLevelup = new Learnset7(levelupGarcFiles[pkm.ID]);
                for (int i = 0; i < currentLevelup.Count; i++)
                {
                    var levelupReference = new LevelupMoveReference();
                    levelupReference.Level = currentLevelup.Levels[i];
                    levelupReference.ID = currentLevelup.Moves[i];
                    levelupReference.Name = moveNames[levelupReference.ID];
                    pkmLevelup.Add(levelupReference);
                }
                pkm.MoveLevelUp = pkmLevelup;

                // TMs
                var pkmTms = new List<Models.Gen7.MoveReference>();
                for (int i = 0; i < TMs.Length; i++)
                {
                    if (item.TMHM[i])
                    {
                        pkmTms.Add(new Models.Gen7.MoveReference { ID = TMs[i], Name = moveNames[TMs[i]] });
                    }
                }
                pkm.MoveTMs = pkmTms;

                // Egg moves
                var eggMoves = new List<Models.Gen7.MoveReference>();
                var currentEgg = new EggMoves7(eggmoveGarcFiles[pkm.ID]);
                for (int i = 0; i < currentEgg.Count; i++)
                {
                    var eggReference = new Models.Gen7.MoveReference();
                    eggReference.ID = currentEgg.Moves[i];
                    eggReference.Name = moveNames[eggReference.ID];
                    eggMoves.Add(eggReference);
                }
                pkm.MoveEgg = eggMoves;

                // Tutors
                var pkmTutors = new List<Models.Gen7.MoveReference>();
                for (int i = 0; i < tutormoves.Length; i++)
                {
                    if (item.TypeTutors[i])
                    {
                        pkmTutors.Add(new Models.Gen7.MoveReference { ID = tutormoves[i], Name = moveNames[tutormoves[i]] });
                    }
                }
                pkm.MoveTutors = pkmTutors;

                // Evolutions
                var evo = new EvolutionSet7(evolutionGarcFiles[pkm.ID]);
                var currentEvolutionMethods = new List<Models.Gen7.EvolutionMethod>();
                for (int i = 0; i < evo.PossibleEvolutions.Length; i++)
                {
                    if (evo.PossibleEvolutions[i].Argument == 0 && evo.PossibleEvolutions[i].Form == 0 && evo.PossibleEvolutions[i].Level == 0 && evo.PossibleEvolutions[i].Method == 0 && evo.PossibleEvolutions[i].Species == 0)
                    {
                        // Method is null
                        continue;
                    }

                    var evoMethod = new Models.Gen7.EvolutionMethod();
                    evoMethod.Form = evo.PossibleEvolutions[i].Form;
                    evoMethod.Level = evo.PossibleEvolutions[i].Level;
                    evoMethod.Method = evolutionMethods[evo.PossibleEvolutions[i].Method];
                    evoMethod.TargetPokemon = new Models.Gen7.PokemonReference { ID = evo.PossibleEvolutions[i].Species, Name = species[evo.PossibleEvolutions[i].Species] };

                    // Parameter
                    int cv = evolutionMethodCase[evo.PossibleEvolutions[i].Method];
                    int param = evo.PossibleEvolutions[i].Argument;
                    switch (cv)
                    {
                        case 0: // No Parameter Required
                            { evoMethod.ParameterString = ""; break; }
                        case 1: // Level
                            { evoMethod.ParameterString = ""; break; }
                        case 2: // Items
                            { evoMethod.ParameterReference = new ItemReference { ID = param, Name = items[param] }; break; }
                        case 3: // Moves
                            { evoMethod.ParameterReference = new Models.Gen7.MoveReference { ID = param, Name = moveNames[param] }; break; }
                        case 4: // Species
                            { evoMethod.ParameterReference = new Models.Gen7.PokemonReference { ID = param, Name = species[param] }; break; }
                        case 5: // 0-255 (Beauty)
                            { evoMethod.ParameterString = param.ToString(); break; }
                        case 6:
                            { evoMethod.ParameterReference = new TypeReference { ID = param, Name = types[param] }; break; }
                        case 7: // Version
                            { evoMethod.ParameterString = param.ToString(); break; }
                    }
                    currentEvolutionMethods.Add(evoMethod);
                }
                pkm.Evolutions = currentEvolutionMethods;

                // Sun/Moon specific
                var sm = (PersonalInfoSM)item;
                pkm.EscapeRate = sm.EscapeRate;
                if (sm.SpecialZ_Item != ushort.MaxValue)
                {
                    pkm.ZItem = new ItemReference { ID = sm.SpecialZ_Item, Name = items[sm.SpecialZ_Item] };
                }
                pkm.ZBaseMove = new Models.Gen7.MoveReference { ID = sm.SpecialZ_BaseMove, Name = moveNames[sm.SpecialZ_BaseMove] };
                pkm.ZMove = new Models.Gen7.MoveReference { ID = sm.SpecialZ_ZMove, Name = moveNames[sm.SpecialZ_ZMove] };
                pkm.LocalVariant = sm.LocalVariant;

                pkms.Add(pkm);
            }
            data.Pokemon = pkms;

            // Moves
            var moves = new List<Models.Gen7.Move>();
            var MoveCategories = new string[] { "Status", "Physical", "Special", };
            var StatCategories = new string[] { "None", "Attack", "Defense", "Special Attack", "Special Defense", "Speed", "Accuracy", "Evasion", "All", };
            var TargetingTypes =
                new string[] { "Single Adjacent Ally/Foe",
                    "Any Ally", "Any Adjacent Ally", "Single Adjacent Foe", "Everyone but User", "All Foes",
                    "All Allies", "Self", "All Pokemon on Field", "Single Adjacent Foe (2)", "Entire Field",
                    "Opponent's Field", "User's Field", "Self",
                };
            var InflictionTypes =
                new string[] { "None",
                    "Paralyze", "Sleep", "Freeze", "Burn", "Poison",
                    "Confusion", "Attract", "Capture", "Nightmare", "Curse",
                    "Taunt", "Torment", "Disable", "Yawn", "Heal Block",
                    "?", "Detect", "Leech Seed", "Embargo", "Perish Song",
                    "Ingrain",
                };
            var MoveQualities =
                new string[] { "Only DMG",
                "No DMG -> Inflict Status", "No DMG -> -Target/+User Stat", "No DMG | Heal User", "DMG | Inflict Status", "No DMG | STATUS | +Target Stat",
                "DMG | -Target Stat", "DMG | +User Stat", "DMG | Absorbs DMG", "One-Hit KO", "Affects Whole Field",
                "Affect One Side of the Field", "Forces Target to Switch", "Unique Effect",  };

            foreach (var item in config.Moves)
            {
                var move = new Models.Gen7.Move();

                move.ID = moves.Count;
                move.Name = moveNames[move.ID];

                // Move data
                string flavor = moveflavor[move.ID].Replace("\\n", Environment.NewLine);
                move.Description = flavor;

                if (types.Length > item.Type)
                {
                    move.Type = new TypeReference { ID = item.Type, Name = types[item.Type] };
                }
                else
                {
                    move.Type = new TypeReference { ID = 0, Name = types[0] };
                }

                if (MoveQualities.Length > item.Quality)
                {
                    move.Qualities = MoveQualities[item.Quality];
                }
                else
                {
                    move.Qualities = "None";
                }

                if (MoveCategories.Length > item.Category)
                {
                    move.Category = MoveCategories[item.Category];
                }
                else
                {
                    move.Category = "None";
                }
                
                move.Power = item.Power;
                move.Accuracy = item.Power;
                move.PP = item.PP;
                move.Priority = item.Priority;
                move.HitMin = item.HitMin;
                move.HitMax = item.HitMax;
                var inflictVal = item.Inflict;
                if (inflictVal < 0)
                {
                    move.Inflict = InflictionTypes.Last();
                }
                else  if (InflictionTypes.Length > inflictVal)
                {
                    move.Inflict = InflictionTypes[inflictVal];
                }
                else
                {
                    move.Inflict = "None";
                }
                
                move.InflictChance = item.InflictPercent;
                move.UnknownB = item._0xB; // 0xB ~ Something to deal with skipImmunity
                move.TurnMin = item.TurnMin;
                move.TurnMax = item.TurnMax;
                move.CritStage = item.CritStage;
                move.Flinch = item.Flinch;
                move.Effect = item.Effect;
                move.Recoil = item.Recoil;
                if (item.Healing.Full)
                {
                    move.Heal = "Full";
                }
                else if (item.Healing.Half)
                {
                    move.Heal = "Half";
                }
                else if (item.Healing.Quarter)
                {
                    move.Heal = "Quarter";
                }
                else
                {
                    move.Heal = $"{item.Healing.Val} HP";
                }

                if (TargetingTypes.Length > item.Targeting)
                {
                    move.Targeting = TargetingTypes[item.Targeting];
                }
                else
                {
                    move.Targeting = "None";
                }

                if (StatCategories.Length > item.Stat1)
                {
                    move.Stat1 = StatCategories[item.Stat1];
                }
                else
                {
                    move.Stat1 = "None";
                }

                if (StatCategories.Length > item.Stat2)
                {
                    move.Stat2 = StatCategories[item.Stat2];
                }
                else
                {
                    move.Stat2 = "None";
                }

                if (StatCategories.Length > item.Stat3)
                {
                    move.Stat3 = StatCategories[item.Stat3];
                }
                else
                {
                    move.Stat3 = "None";
                }

                move.Stat1Num = item.Stat1Stage;
                move.Stat2Num = item.Stat1Stage;
                move.Stat3Num = item.Stat1Stage;
                move.Stat1Percent = item.Stat1Percent;
                move.Stat2Percent = item.Stat2Percent;
                move.Stat3Percent = item.Stat3Percent;

                // Unknown (Bitflag Related for stuff like Contact and Extra Move Effects)
                move.Unknown20 = item._0x20; // 0x20
                move.Unknown21 = item._0x21; // 0x21

                // Pokemon references
                move.PokemonThroughLevelUp = new List<Models.Gen7.LevelupPokemonReference>();
                move.PokemonThroughTM = new List<Models.Gen7.PokemonReference>();
                move.PokemonThroughEgg = new List<Models.Gen7.PokemonReference>();
                move.PokemonThroughTutor = new List<Models.Gen7.PokemonReference>();
                foreach (var pkm in data.Pokemon)
                {
                    // Levelup
                    var referenceMovesLevel = pkm.MoveLevelUp.Where(x => x.ID == move.ID);
                    if (referenceMovesLevel.Any())
                    {
                        var pkmReference = new Models.Gen7.LevelupPokemonReference { ID = pkm.ID, Name = pkm.Name };
                        pkmReference.Levels = referenceMovesLevel.Select(x => x.Level).ToList();
                        move.PokemonThroughLevelUp.Add(pkmReference);
                    }

                    // TM
                    var referenceMovesTM = pkm.MoveTMs.Where(x => x.ID == move.ID);
                    if (referenceMovesTM.Any())
                    {
                        var pkmReference = new Models.Gen7.PokemonReference { ID = pkm.ID, Name = pkm.Name };
                        move.PokemonThroughTM.Add(pkmReference);
                    }

                    // Egg
                    var referenceMovesEgg = pkm.MoveEgg.Where(x => x.ID == move.ID);
                    if (referenceMovesEgg.Any())
                    {
                        var pkmReference = new Models.Gen7.PokemonReference { ID = pkm.ID, Name = pkm.Name };
                        move.PokemonThroughEgg.Add(pkmReference);
                    }

                    // Tutor
                    var referenceMovesTutor = pkm.MoveTutors.Where(x => x.ID == move.ID);
                    if (referenceMovesTutor.Any())
                    {
                        var pkmReference = new Models.Gen7.PokemonReference { ID = pkm.ID, Name = pkm.Name };
                        move.PokemonThroughTutor.Add(pkmReference);
                    }
                }

                moves.Add(move);
            }
            data.Moves = moves;

            return data;
        }

        static string BuildAndReturnTemplate<T>(object model) where T : class
        {
            // Verify the type is correct
            var templateType = typeof(T);
            var sessionProperty = templateType.GetProperty("Session");
            var initializeMethod = templateType.GetMethod("Initialize", new Type[] { });
            var transformMethod = templateType.GetMethod("TransformText", new Type[] { });

            if (sessionProperty == null || sessionProperty.PropertyType != typeof(IDictionary<string, object>))
            {
                throw new ArgumentException("Type parameter must have a Session property of type IDictionary<string, object>.", nameof(T));
            }
            if (initializeMethod == null || initializeMethod.GetParameters().Length > 0)
            {
                throw new ArgumentException("Type parameter must have a Initialize() method with no parameters.", nameof(T));
            }
            if (transformMethod == null || transformMethod.GetParameters().Length > 0)
            {
                throw new ArgumentException("Type parameter must have a TransformText() method with no parameters.", nameof(T));
            }

            // Set up the template
            var templateInstance = templateType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var session = new TextTemplatingSession();
            sessionProperty.SetValue(templateInstance, session);
            session["Model"] = model;
            initializeMethod.Invoke(templateInstance, new object[] { });

            // Run the template
            var resultText = transformMethod.Invoke(templateInstance, new object[] { }) as string;
            return resultText;
        }

        static void BuildSM(string smPath, string outputFilename)
        {
            // Extract ROM if needed
            var tempDir = "smROM";
            if (File.Exists(smPath))
            {
                // It's needed
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                RunProgram("3dstool.exe", $"-xtf 3ds \"{smPath}\" -0 Partition0.bin");
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin --exefs ExeFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                RunProgram("3dstool.exe", $"-xutf exefs ExeFS.bin --exefs-dir \"{tempDir}/ExeFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");
                File.Delete("ExeFS.bin");
                smPath = tempDir;
            }

            var data = LoadSunMoonData(smPath);

            var output = new List<Category>();

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = "Pokemon";
            catPkm.Records = new List<Record>();
            foreach (var item in data.Pokemon)
            {
                catPkm.Records.Add(new Record
                {
                    Title = item.ID.ToString().PadLeft(3, '0') + " " + item.Name,
                    Content = BuildAndReturnTemplate<Views.Gen7.Pokemon.Details>(item),
                    InternalName = $"pkm-" + item.ID
                });
            }
            output.Add(catPkm);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));

            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }

        static async Task BuildEOS(string eosPath, string outputFilename)
        {
            var provider = new PhysicalIOProvider();
            var romDir = "eos-rawfiles";
            using (var eosROM = new GenericNDSRom())
            {
                await eosROM.OpenFile(eosPath, provider);
                await eosROM.Unpack(romDir, provider);
            }

            var data = await LoadEosData(romDir);

            var output = new List<Category>();

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = "Pokemon";
            catPkm.Records = new List<Record>();
            catPkm.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Pokemon.Index>(data.Pokemon),
                InternalName = $"pkm-index"
            });
            foreach (var item in data.Pokemon)
            {
                catPkm.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Pokemon.Details>(item),
                    InternalName = $"pkm-" + item.ID
                });
            }
            output.Add(catPkm);

            // Moves
            var catMoves = new Category();
            catMoves.Name = "Moves";
            catMoves.Records = new List<Record>();
            catMoves.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Moves.Index>(data.Moves),
                InternalName = $"move-index"
            });
            foreach (var item in data.Moves)
            {
                catMoves.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Moves.Details>(item),
                    InternalName = $"move-" + item.ID
                });
            }
            output.Add(catMoves);

            // Types
            var catTypes = new Category();
            catTypes.Name = "Types";
            catTypes.Records = new List<Record>();
            catTypes.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Types.Index>(data.Types),
                InternalName = $"type-index"
            });
            foreach (var item in data.Types)
            {
                catTypes.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Types.Details>(item),
                    InternalName = $"type-" + item.ID
                });
            }
            output.Add(catTypes);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));

            // Cleanup
            if (Directory.Exists(romDir))
            {
                Directory.Delete(romDir, true);
            }
        }

        static async Task BuildPSMD(string psmdPath, string outputFilename)
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
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin --exefs ExeFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                RunProgram("3dstool.exe", $"-xutf exefs ExeFS.bin --exefs-dir=\"{tempDir}/ExeFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");
                File.Delete("ExeFS.bin");
                psmdPath = tempDir;
            }            

            var data = await LoadPsmdData(psmdPath);

            var output = new List<Category>();

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = "Pokemon";
            catPkm.Records = new List<Record>();
            catPkm.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Pokemon.Index>(data.Pokemon),
                InternalName = "pkm-index"
            });
            foreach (var item in data.Pokemon)
            {
                catPkm.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Pokemon.Details>(new PokemonDetailsViewModel(item, data)),
                    InternalName = $"pkm-" + item.ID
                });
            }
            output.Add(catPkm);

            // Moves
            var catMoves = new Category();
            catMoves.Name = "Moves";
            catMoves.Records = new List<Record>();
            catMoves.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Moves.Index>(data.Moves),
                InternalName = "move-index"
            });
            foreach (var item in data.Moves)
            {
                catMoves.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Moves.Details>(new MoveDetailsViewModel(item, data)),
                    InternalName = $"move-" + item.ID,
                });
            }
            output.Add(catMoves);

            // Abilities
            var catAbilities = new Category();
            catAbilities.Name = "Abilities";
            catAbilities.Records = new List<Record>();
            catAbilities.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Abilities.Index>(data.Abilities),
                InternalName = "ability-index"
            });
            foreach (var item in data.Abilities)
            {
                catAbilities.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Abilities.Details>(new AbilityDetailsViewModel(item, data)),
                    InternalName = $"ability-" + item.ID,
                });
            }
            output.Add(catAbilities);

            // Types
            var catTypes = new Category();
            catTypes.Name = "Types";
            catTypes.Records = new List<Record>();
            catTypes.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.PSMD.Types.Index>(data.Types),
                InternalName = "type-index"
            });
            foreach (var item in data.Types)
            {
                catTypes.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.PSMD.Types.Details>(new TypeDetailsViewModel(item, data)),
                    InternalName = $"type-" + item.ID
                });
            }
            output.Add(catTypes);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));

            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }

        static void Main(string[] args)
        {
            var name = args[0];
            var filename = args[1];
            var outputFilename = args[2];

            switch (name.ToLower())
            {
                case "moon":
                    BuildSM(filename, outputFilename);
                    break;
                case "psmd":
                    BuildPSMD(filename, outputFilename).Wait();
                    break;
                case "eos":
                    BuildEOS(filename, outputFilename).Wait();
                    break;
                default:
                    Console.WriteLine("Usage: ProjectPokemonPokedex.exe <eos|psmd|moon> <RomFilename> <OutputFilename>");
                    break;
            }            
        }
    }
}
