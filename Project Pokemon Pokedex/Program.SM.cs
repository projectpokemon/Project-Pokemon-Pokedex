using IPS_Pages_Publisher.Interfaces;
using Newtonsoft.Json;
using pk3DS.Core;
using pk3DS.Core.Structures;
using pk3DS.Core.Structures.PersonalInfo;
using ProjectPokemon.Pokedex.Models.Gen7;
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
        private static readonly byte[] SignatureSM = { 0x03, 0x40, 0x03, 0x41, 0x03, 0x42, 0x03, 0x43, 0x03 }; // tail end of item::ITEM_CheckBeads
        internal static void getTMHMList(string ExeFSPath, ref ushort[] TMs, bool isUltra)
        {
            if (ExeFSPath == null) return;
            string exeFsFile = Directory.GetFiles(ExeFSPath).FirstOrDefault(x => x.Contains("code"));
            if (!File.Exists(exeFsFile) || !Path.GetFileNameWithoutExtension(exeFsFile).Contains("code"))
            {
                throw new Exception($"Couldn't find code.bin in path '{ExeFSPath}'");
            }
            byte[] data = File.ReadAllBytes(exeFsFile);
            int dataoffset = Util.IndexOfBytes(data, SignatureSM, 0x400000, 0) + SignatureSM.Length;

            if (isUltra)
            {
                dataoffset += 0x22;
            }
            
            if (data.Length % 0x200 != 0) return;

            List<ushort> tms = new List<ushort>();

            for (int i = 0; i < 100; i++) // TMs stored sequentially
                tms.Add(BitConverter.ToUInt16(data, dataoffset + 2 * i));
            TMs = tms.ToArray();
        }

        private static string[][] getFormList(GameConfig config, string[] species)
        {
            int MaxSpeciesId;
            if (config.Version == GameVersion.SM)
            {
                MaxSpeciesId = 802;
            }
            else if (config.Version == GameVersion.USUM)
            {
                MaxSpeciesId = 807;
            }
            else
            {
                throw new Exception("Unsupported version");
            }

            string[] gendersymbols = { "♂", "♀", "-" };
            var table = config.Personal.Table;
            string[][] FormList = new string[MaxSpeciesId + 1][];
            var typeNames = PKHeX.Core.Util.GetTypesList("en");
            var formNames = PKHeX.Core.Util.GetFormsList("en");
            for (int i = 0; i <= MaxSpeciesId; i++)
            {
                string[] formStrings;
                try
                {
                    int FormCount = (table.Length > i ? table[i] : table[0]).FormeCount;
                    FormList[i] = new string[FormCount];

                    if (FormCount <= 0) continue;

                    // PKHeX form list
                    formStrings = PKHeX.Core.PKX.GetFormList(i,
                    typeNames,
                    formNames, gendersymbols, 7);

                    FormList[i][0] = species[i];

                    for (int j = 1; j < FormCount; j++)
                    {
                        if (j < formStrings.Length)
                        {
                            FormList[i][j] = $"{species[i]} ({formStrings[j]})";
                        }
                        else
                        {
                            FormList[i][j] = $"{species[i]} (Form {j})";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed when i = " + i.ToString());
                    Console.WriteLine("typeNames.Length = " + typeNames.Length);
                    Console.WriteLine("formNames.Length = " + formNames.Length);
                    Console.WriteLine("Message: " + ex.ToString());
                    Console.WriteLine("Stack trace: " + ex.StackTrace);
                    throw;
                }   
            }
            return FormList;
        }

        /// <summary>
        /// Gets names for all Pokemon IDs, even alt forms
        /// </summary>
        private static string[] GetPokemonEntryNames(GameConfig config, string[] speciesNames)
        {
            var altForms = getFormList(config, speciesNames);
            int[] baseForms, formVal;
            return config.Personal.getPersonalEntryList(altForms, speciesNames, config.MaxSpeciesID, out baseForms, out formVal);
        }

        // exefs stuff
        private static readonly byte[] ExefsSignature =
        {
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0xC3, 0x00, 0x00, 0x00, 0xCB, 0x00, 0x00, 0x00, 0xD3, 0x00, 0x00, 0x00, 0xDB, 0x00, 0x00, 0x00,
            0xF3, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
        };

        private static void LoadTypeEffectiveness(SMDataCollection data, byte[] exefs)
        {
            var typeEffectivenessOffset = Util.IndexOfBytes(exefs, ExefsSignature, 0x400000, 0) + ExefsSignature.Length;
            var chart = new byte[18 * 18];
            Array.Copy(exefs, typeEffectivenessOffset, chart, 0, chart.Length);
            data.TypeEffectiveness = new TypeEffectivenessChart(chart);
        }

        private static void LoadTypes(SMDataCollection data, string[] typeNames)
        {
            const int numTypes = 18;
            var pkmTypes = new List<PkmType>();
            for (int i = 0; i < numTypes; i++)
            {
                var type = new PkmType();
                type.ID = i;
                type.Name = typeNames[i];
                type.Pokemon = new List<PokemonReference>();
                type.Moves = new List<MoveReference>();
                pkmTypes.Add(type);
            }
            data.Types = pkmTypes;
        }

        private static void LoadPokemon(SMDataCollection data, GameConfig config, string rawFilesDir, string[] speciesNames, string[] typeNames, string[] itemNames, string[] abilityNames, string[] moveNames, string[] EXPGroups, string[] eggGroups, string[] colors, string[] pokemonClassifications, string[] pokedexEntries1, string[] pokedexEntries2, string[][] altForms)
        {
            // Load TMs
            var TMs = new ushort[0];
            getTMHMList(Path.Combine(rawFilesDir, "ExeFS"), ref TMs, data.IsUltra);

            // Load Pokemon
            // - Load Level-up GARC
            var levelupGarcFiles = config.GetGARCData("levelup").Files;
            string[] pokemonEntryNames = GetPokemonEntryNames(config, speciesNames);
            data.AltFormStrings = altForms;

            // - Load Egg move GARC
            var eggmoveGarcFiles = config.GetGARCData("eggmove").Files;


            // - Load Mega Evolution GARC
            var megaEvoGarcFiles = config.GetGARCData("megaevo").Files;

            // - Consolidate data
            var pkms = new List<Pokemon>();
            foreach (var item in config.Personal.Table)
            {
                var pkm = new Pokemon(data);

                pkm.ID = pkms.Count;
                if (pokemonEntryNames.Length > pkm.ID)
                {
                    pkm.Name = pokemonEntryNames[pkm.ID];
                }
                else
                {
                    pkm.Name = "(Unnamed)";
                }

                if (pokemonClassifications.Length > pkm.ID)
                {
                    pkm.Classification = pokemonClassifications[pkm.ID];
                }
                else
                {
                    pkm.Classification = "N/A";
                }

                if (pokedexEntries1.Length > pkm.ID || string.IsNullOrEmpty(pokedexEntries1[pkm.ID]))
                {
                    pkm.PokedexTextSun = pokedexEntries1[pkm.ID];
                }
                else
                {
                    pkm.PokedexTextSun = "N/A";
                }

                if (pokedexEntries2.Length > pkm.ID || string.IsNullOrEmpty(pokedexEntries2[pkm.ID]))
                {
                    pkm.PokedexTextMoon = pokedexEntries2[pkm.ID];
                }
                else
                {
                    pkm.PokedexTextMoon = "N/A";
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
                pkm.Type1 = new TypeReference { ID = item.Types[0], Name = typeNames[item.Types[0]] };
                pkm.Type2 = new TypeReference { ID = item.Types[1], Name = typeNames[item.Types[1]] };
                pkm.TypeEffectiveness = new TypeEffectivenessList(data.TypeEffectiveness, data.Types, pkm.Type1.ID, pkm.Type2.ID);

                pkm.CatchRate = item.CatchRate;
                pkm.EvoStage = item.EvoStage;

                // Items
                pkm.HeldItem1 = new ItemReference(item.Items[0], itemNames[item.Items[0]]);
                pkm.HeldItem2 = new ItemReference(item.Items[1], itemNames[item.Items[1]]);
                pkm.HeldItem3 = new ItemReference(item.Items[2], itemNames[item.Items[2]]);

                pkm.Gender = item.Gender;
                pkm.HatchCycles = item.HatchCycles;
                pkm.BaseFriendship = item.BaseFriendship;

                pkm.ExpGrowth = EXPGroups[item.EXPGrowth];

                pkm.EggGroup1 = eggGroups[item.EggGroups[0]];
                pkm.EggGroup2 = eggGroups[item.EggGroups[1]];

                pkm.Ability1 = new AbilityReference { ID = item.Abilities[0], Name = abilityNames[item.Abilities[0]] };
                pkm.Ability2 = new AbilityReference { ID = item.Abilities[1], Name = abilityNames[item.Abilities[1]] };
                pkm.AbilityHidden = new AbilityReference { ID = item.Abilities[2], Name = abilityNames[item.Abilities[2]] };

                pkm.FormeCount = item.FormeCount;
                pkm.FormeSprite = item.FormeSprite;

                pkm.Color = colors[item.Color & 0xF];

                pkm.BaseExp = item.BaseEXP;
                pkm.BST = item.BST;

                pkm.Height = ((decimal)item.Height / 100);
                pkm.Weight = ((decimal)item.Weight / 10);

                // Sun/Moon specific
                var sm = (PersonalInfoSM)item;
                pkm.EscapeRate = sm.EscapeRate;
                if (sm.SpecialZ_Item != ushort.MaxValue)
                {
                    pkm.ZItem = new ItemReference(sm.SpecialZ_Item, itemNames[sm.SpecialZ_Item]);
                }
                pkm.ZBaseMove = new MoveReference(sm.SpecialZ_BaseMove, moveNames[sm.SpecialZ_BaseMove], data);
                pkm.ZMove = new MoveReference(sm.SpecialZ_ZMove, moveNames[sm.SpecialZ_ZMove], data);
                pkm.LocalVariant = sm.LocalVariant;

                // Evolutions & forms
                LoadPokemonAltFormReferences(data, pkm, config, pokemonEntryNames, altForms);
                LoadPokemonEvolutions(data, pkm, config, pokemonEntryNames, moveNames, itemNames, typeNames);
                LoadPokemonMegaEvolutions(data, pkm, config, itemNames);
                
                // Moves
                // - Level-up
                var pkmLevelup = new List<LevelupMoveReference>();
                var currentLevelup = new Learnset6(levelupGarcFiles[pkm.ID]);
                for (int i = 0; i < currentLevelup.Count; i++)
                {
                    var levelupReference = new LevelupMoveReference(
                        currentLevelup.Moves[i],
                        moveNames[currentLevelup.Moves[i]],
                        currentLevelup.Levels[i],
                        data);
                    pkmLevelup.Add(levelupReference);
                }
                pkm.MoveLevelUp = pkmLevelup;

                // - TMs
                var pkmTms = new List<MoveReference>();
                for (int i = 0; i < TMs.Length; i++)
                {
                    if (item.TMHM[i])
                    {
                        pkmTms.Add(new MoveReference(TMs[i], moveNames[TMs[i]], data));
                    }
                }
                pkm.MoveTMs = pkmTms;

                // - Egg moves
                var eggMoves = new List<Models.Gen7.MoveReference>();
                var currentEgg = new EggMoves7(eggmoveGarcFiles[pkm.ID]);
                for (int i = 0; i < currentEgg.Count; i++)
                {
                    var eggReference = new MoveReference(currentEgg.Moves[i], moveNames[currentEgg.Moves[i]], data);
                    eggMoves.Add(eggReference);
                }
                pkm.MoveEgg = eggMoves;

                // - Tutors
                var tutormoves = new ushort[] { 520, 519, 518, 338, 307, 308, 434, 620 };
                var pkmTutors = new List<Models.Gen7.MoveReference>();
                for (int i = 0; i < tutormoves.Length; i++)
                {
                    if (item.TypeTutors[i])
                    {
                        pkmTutors.Add(new MoveReference(tutormoves[i], moveNames[tutormoves[i]], data));
                    }
                }
                pkm.MoveTutors = pkmTutors;

                // Finish
                AddPkmToType(data, pkm);
                pkms.Add(pkm);
            }
            data.Pokemon = pkms;
        }

        private static void LoadPokemonEvolutions(SMDataCollection data, Pokemon pkm, GameConfig config, string[] speciesNames, string[] moveNames, string[] itemNames, string[] typeNames)
        {
            // - Load Evolution GARC
            var evolutionGarcFiles = config.GetGARCData("evolution").Files;
            string[] evolutionMethods =
            {
                "", // No param
                "Level Up with Friendship", // No param
                "Level Up at Morning with Friendship", // No param
                "Level Up at Night with Friendship", // No param
                "Level Up at level {0}", // Level
                "Trade", // No param
                "Trade with Held Item: {0}", // Item
                $"Trade for opposite {speciesNames[588]}/{speciesNames[616]}", // Shelmet&Karrablast  // No param
                "Used Item: {0}", // Item
                "Level Up (Attack > Defense) at level {0}", // Level
                "Level Up (Attack = Defense) at level {0}", // Level
                "Level Up (Attack < Defense) at level {0}", // Level
                "Level Up (Random < 5) at level {0}", // Level
                "Level Up (Random > 5) at level {0}", // Level
                $"Level Up ({speciesNames[291]}) at level {0}", // Ninjask // Level
                $"Level Up ({speciesNames[292]}) at level {0}", // Shedinja // Level
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
                $"{typeNames[16]} Type in Party " + "at level {0}", // Level
                "Overworld Rain at level {0}", // Level
                "Level Up at Morning at level {0}", // Level
                "Level Up at Night at level {0}", // Level
                "Level Up Female (SetForm 1) at level {0}", // Level
                "UNUSED",
                "Level Up Any Time on Version {0}", // Version
                "Level Up Daytime on Version {0}", // Version
                "Level Up Nighttime on Version {0}", // Version
                "Level Up Summit at level {0}", // Level
                 // new in USUM
                "Level Up at Dusk at level {0}",
                "Level Up in Ultra Wormhole at level {0}",
                "Used Item {0} in Ultra Wormhole"
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
                // USUM new
                1, // 40 - Level Up with Condition (???)
                1, // 41 - Level Up with Condition (???)
                2, // 42 - Use Item with Condition (???)
            };

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
                if (evoMethod.Form > -1 )
                {
                    var targetId = GetAltFormId(evo.PossibleEvolutions[i].Species, evo.PossibleEvolutions[i].Form, config);

                    if (!targetId.HasValue) continue; // Form doesn't exist (like for Unown)

                    evoMethod.TargetPokemon = new PokemonReference(targetId.Value, speciesNames[targetId.Value], data);
                }
                else
                {
                    evoMethod.TargetPokemon = new PokemonReference(evo.PossibleEvolutions[i].Species, speciesNames[evo.PossibleEvolutions[i].Species], data);
                }                

                // Parameter
                int cv = evolutionMethodCase[evo.PossibleEvolutions[i].Method];
                int param = evo.PossibleEvolutions[i].Argument;
                switch (cv)
                {
                    case 0: // No Parameter Required
                        { evoMethod.ParameterString = ""; break; }
                    case 1: // Level
                        { evoMethod.ParameterString = evoMethod.Level.ToString(); break; }
                    case 2: // Items
                        { evoMethod.ParameterReference = new ItemReference(param, itemNames[param]); break; }
                    case 3: // Moves
                        { evoMethod.ParameterReference = new MoveReference(param, moveNames[param], data); break; }
                    case 4: // Species
                        { evoMethod.ParameterReference = new PokemonReference(param, speciesNames[param], data); break; }
                    case 5: // 0-255 (Beauty)
                        { evoMethod.ParameterString = param.ToString(); break; }
                    case 6:
                        { evoMethod.ParameterReference = new TypeReference { ID = param, Name = typeNames[param] }; break; }
                    case 7: // Version
                        { evoMethod.ParameterString = param.ToString(); break; }
                }
                currentEvolutionMethods.Add(evoMethod);
            }
            pkm.Evolutions = currentEvolutionMethods;
        }

        private static int? GetAltFormId(int pkmId, int formIndex, GameConfig config)
        {
            var index = config.Personal.Table[pkmId].FormStatsIndex;
            if (formIndex == 0)
            {
                return pkmId;
            }
            if (index == 0)
            {
                return null;
            }
            return index + formIndex - 1;
        }

        private static void LoadPokemonAltFormReferences(SMDataCollection data, Pokemon pkm, GameConfig config, string[] speciesNames, string[][] altForms)
        {
            if (config.Personal.Table.Length <= pkm.ID || altForms.Length <= pkm.ID)
            {
                return;
            }
            
            for (int j = 1; j < altForms[pkm.ID].Length; j++)
            {
                var referenceId = GetAltFormId(pkm.ID, j, config);
                if (referenceId.HasValue)
                {
                    pkm.AltForms.Add(new PokemonReference(referenceId.Value, speciesNames[referenceId.Value], data));
                }                
            }                
        }

        private static byte[][] _megaGarcFiles = null;
        private static void LoadPokemonMegaEvolutions(SMDataCollection data, Pokemon pkm, GameConfig config, string[] itemNames)
        {
            if (_megaGarcFiles == null)
            {
                _megaGarcFiles = config.GetGARCData("megaevo").Files;
            }

            if (_megaGarcFiles.Length <= pkm.ID)
            {
                // We're outside the range of mega evolution files. This probably means this Pokemon is an alt form.
                return;
            }

            var megaEvo = new MegaEvolutions(_megaGarcFiles[pkm.ID]);

            if (megaEvo.Method[0] == 1)
            {
                pkm.Evolutions.Add(new Models.Gen7.EvolutionMethod
                {
                    Form = -1,
                    Level = 0,
                    Method = "Mega evolve with {0}",
                    ParameterReference = new ItemReference((int)megaEvo.Argument[0], itemNames[(int)megaEvo.Argument[0]]),
                    TargetPokemon = pkm.AltForms[megaEvo.Form[0] - 1]
                });
                pkm.MegaEvolutions.Add(pkm.AltForms[megaEvo.Form[0] - 1]);
            }

            if (megaEvo.Method[1] == 1)
            {
                pkm.Evolutions.Add(new Models.Gen7.EvolutionMethod
                {
                    Form = -1,
                    Level = 0,
                    Method = "Mega evolve with {0}",
                    ParameterReference = new ItemReference((int)megaEvo.Argument[1], itemNames[(int)megaEvo.Argument[1]]),
                    TargetPokemon = pkm.AltForms[megaEvo.Form[1] - 1]
                });
                pkm.MegaEvolutions.Add(pkm.AltForms[megaEvo.Form[1] - 1]);
            }                        
            
        }

        private static void AddPkmToType(SMDataCollection data, Pokemon pkm)
        {
            data.Types[pkm.Type1.ID].Pokemon.Add(new PokemonReference(pkm));
            if (pkm.Type1.ID != pkm.Type2.ID)
            {
                data.Types[pkm.Type2.ID].Pokemon.Add(new PokemonReference(pkm));
            }
        }

        private static void LoadMoves(SMDataCollection data, GameConfig config, string[] moveNames, string[] moveflavor, string[] typeNames)
        {
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

                if (typeNames.Length > item.Type)
                {
                    move.Type = new TypeReference { ID = item.Type, Name = typeNames[item.Type] };
                }
                else
                {
                    move.Type = new TypeReference { ID = 0, Name = typeNames[0] };
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
                else if (InflictionTypes.Length > inflictVal)
                {
                    move.Inflict = InflictionTypes[inflictVal];
                }
                else
                {
                    move.Inflict = "None";
                }

                move.InflictChance = item.InflictPercent;
                move.TurnMin = item.TurnMin;
                move.TurnMax = item.TurnMax;
                move.CritStage = item.CritStage;
                move.Flinch = item.Flinch;
                move.Effect = item.EffectSequence;
                move.Recoil = item.Recoil;
                if (item.Healing == Heal.Full)
                {
                    move.Heal = "Full";
                }
                else if (item.Healing == Heal.Half)
                {
                    move.Heal = "Half";
                }
                else if (item.Healing == Heal.Quarter)
                {
                    move.Heal = "Quarter";
                }
                else
                {
                    move.Heal = $"{(byte)item.Healing} HP";
                }

                if (TargetingTypes.Length > (byte)item.Target)
                {
                    move.Targeting = TargetingTypes[(byte)item.Target];
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

                // Pokemon references
                move.PokemonThroughLevelUp = new List<LevelupPokemonReference>();
                move.PokemonThroughTM = new List<PokemonReference>();
                move.PokemonThroughEgg = new List<PokemonReference>();
                move.PokemonThroughTutor = new List<PokemonReference>();
                foreach (var pkm in data.Pokemon)
                {
                    // Levelup
                    var referenceMovesLevel = pkm.MoveLevelUp.Where(x => x.ID == move.ID);
                    if (referenceMovesLevel.Any())
                    {
                        var pkmReference = new LevelupPokemonReference(pkm);
                        pkmReference.Levels = referenceMovesLevel.Select(x => x.Level).ToList();
                        move.PokemonThroughLevelUp.Add(pkmReference);
                    }

                    // TM
                    var referenceMovesTM = pkm.MoveTMs.Where(x => x.ID == move.ID);
                    if (referenceMovesTM.Any())
                    {
                        var pkmReference = new PokemonReference(pkm);
                        move.PokemonThroughTM.Add(pkmReference);
                    }

                    // Egg
                    var referenceMovesEgg = pkm.MoveEgg.Where(x => x.ID == move.ID);
                    if (referenceMovesEgg.Any())
                    {
                        var pkmReference = new PokemonReference(pkm);
                        move.PokemonThroughEgg.Add(pkmReference);
                    }

                    // Tutor
                    var referenceMovesTutor = pkm.MoveTutors.Where(x => x.ID == move.ID);
                    if (referenceMovesTutor.Any())
                    {
                        var pkmReference = new PokemonReference(pkm);
                        move.PokemonThroughTutor.Add(pkmReference);
                    }
                }

                // Add to lists
                moves.Add(move);
                data.Types[move.Type.ID].Moves.Add(new MoveReference(move, data));
            }
            data.Moves = moves;
        }

        public static SMDataCollection LoadSunMoonData(string rawFilesDir, bool isUltra)
        {
            // Extract ROM if needed
            var tempDir = "smROM";
            if (!Directory.Exists(rawFilesDir))
            {
                // It's needed
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                RunProgram("3dstool.exe", $"-xtf 3ds \"{rawFilesDir}\" -0 Partition0.bin");
                RunProgram("3dstool.exe", $"-xtf cxi Partition0.bin --romfs RomFS.bin --exefs ExeFS.bin");
                RunProgram("3dstool.exe", $"-xtf romfs RomFS.bin --romfs-dir \"{tempDir}/RomFS\"");
                RunProgram("3dstool.exe", $"-xutf exefs ExeFS.bin --exefs-dir \"{tempDir}/ExeFS\"");
                File.Delete("Partition0.bin");
                File.Delete("RomFS.bin");
                File.Delete("ExeFS.bin");
                rawFilesDir = tempDir;
            }

            var data = new SMDataCollection();
            data.IsUltra = isUltra;
            byte[] exefs;
            if (File.Exists(Path.Combine(rawFilesDir, "ExeFS", ".code.bin")))
            {
                exefs = File.ReadAllBytes(Path.Combine(rawFilesDir, "ExeFS", ".code.bin"));
            }
            else
            {
                exefs = File.ReadAllBytes(Path.Combine(rawFilesDir, "ExeFS", "code.bin"));
            }

            GameConfig config;
            if (isUltra)
            {
                config = new GameConfig(GameVersion.USUM);
            }
            else
            {
                config = new GameConfig(GameVersion.SM);
            }
            config.RemapCharacters = true;
            
            config.Initialize(Path.Combine(rawFilesDir, "RomFS"), Path.Combine(rawFilesDir, "ExeFS"), lang: 2); // Language index 2 is English

            // Load strings
            var items = config.getText(TextName.ItemNames);
            var moveNames = config.getText(TextName.MoveNames);
            var moveflavor = config.getText(TextName.MoveFlavor);
            var speciesNames = config.getText(TextName.SpeciesNames);
            var speciesClassifications = config.getText(TextName.SpeciesClassifications);
            var pokedexEntries1 = config.getText(TextName.PokedexEntry1);
            var pokedexEntries2 = config.getText(TextName.PokedexEntry2);
            var abilities = config.getText(TextName.AbilityNames);
            var forms = config.getText(TextName.Forms);
            var typeNames = config.getText(TextName.Types);
            var EXPGroups = new string[] { "Medium-Fast", "Erratic", "Fluctuating", "Medium-Slow", "Fast", "Slow" };
            var eggGroups = new string[] { "---", "Monster", "Water 1", "Bug", "Flying", "Field", "Fairy", "Grass", "Human-Like", "Water 3", "Mineral", "Amorphous", "Water 2", "Ditto", "Dragon", "Undiscovered" };
            var colors = new string[] { "Red", "Blue", "Yellow", "Green", "Black", "Brown", "Purple", "Gray", "White", "Pink" };
            var altForms = getFormList(config, speciesNames);

            // Load stuff
            LoadTypeEffectiveness(data, exefs);
            LoadTypes(data, typeNames);
            LoadPokemon(data, config, rawFilesDir, speciesNames, typeNames, items, abilities, moveNames, EXPGroups, eggGroups, colors, speciesClassifications, pokedexEntries1, pokedexEntries2, altForms);
            LoadMoves(data, config, moveNames, moveflavor, typeNames);

            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            return data;
        }

        static void BuildSM(SMDataCollection data, string outputFilename, bool isUltra)
        {    
            var output = new List<Category>();

            string gameTag;
            if (isUltra)
            {
                gameTag = "usum";
            }
            else
            {
                gameTag = "sm";
            }

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = $"{gameTag}-Pokemon";
            catPkm.Records = new List<Record>();            
            foreach (var item in data.Pokemon)
            {
                // Exclude Egg
                if (item.ID == 0) continue;

                catPkm.Records.Add(new Record
                {
                    Title = item.ID.ToString().PadLeft(3, '0') + " " + item.Name,
                    Content = BuildAndReturnTemplate<Views.Gen7.Pokemon.Details>(item),
                    InternalName = $"{gameTag}-pkm-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catPkm.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.Gen7.Pokemon.Index>(data.Pokemon),
                InternalName = $"{gameTag}-pkm-index"
            });
            output.Add(catPkm);

            // Egg Groups
            var catEggGroups = new Category();
            catEggGroups.Name = $"{gameTag}-EggGroups";
            catEggGroups.Records = new List<Record>();
            var eggGroups = data.GetEggGroups();
            foreach (var item in eggGroups)
            {
                catEggGroups.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.Gen7.EggGroups.Details>(item),
                    InternalName = $"{gameTag}-egggroup-" + item.Name,
                    Tags = new[] { item.Name }
                });
            }
            catEggGroups.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.Gen7.EggGroups.Index>(eggGroups),
                InternalName = $"{gameTag}-egggroup-index"
            });
            output.Add(catEggGroups);

            // Moves
            var catMove = new Category();
            catMove.Name = $"{gameTag}-Moves";
            catMove.Records = new List<Record>();
            foreach (var item in data.Moves)
            {
                catMove.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.Gen7.Moves.Details>(item),
                    InternalName = $"{gameTag}-move-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catMove.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.Gen7.Moves.Index>(data.Moves),
                InternalName = $"{gameTag}-move-index"
            });
            output.Add(catMove);

            // Types
            var catType = new Category();
            catType.Name = $"{gameTag}-Types";
            catType.Records = new List<Record>();
            foreach (var item in data.Types)
            {
                catType.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.Gen7.Types.Details>(item),
                    InternalName = $"{gameTag}-type-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catType.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.Gen7.Types.Index>(data.Types),
                InternalName = $"{gameTag}-type-index"
            });
            output.Add(catType);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));
        }
    }
}
