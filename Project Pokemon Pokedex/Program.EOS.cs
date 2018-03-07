using DotNetNdsToolkit;
using IPS_Pages_Publisher.Interfaces;
using Newtonsoft.Json;
using ProjectPokemon.Pokedex.Models.EOS;
using SkyEditor.Core.IO;
using SkyEditor.ROMEditor.MysteryDungeon.Explorers;
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
        public static async Task<EosDataCollection> LoadEosData(IIOProvider rom)
        {
            var data = new EosDataCollection();

            var languageFile = new LanguageString();
            await languageFile.OpenFile("/data/MESSAGE/text_e.str", rom);

            var moveFile = new waza_p();
            await moveFile.OpenFile("/data/BALANCE/waza_p.bin", rom);

            // Load Types
            var types = new List<PkmType>();
            for (int i = 0; i < 19; i++)
            {
                var t = new PkmType();
                t.ID = i;
                t.Name = languageFile.GetTypeName(i);
                t.Pokemon = new List<PokemonReference>();
                t.Moves = new List<MoveReference>();
                types.Add(t);
            }
            data.Types = types;

            // Read Move Data
            var moves = new List<Move>();
            for (int i = 0; i < moveFile.Moves.Count; i++)
            {
                var m = new Move { ID = i, Name = languageFile.GetMoveName(i), RawData = moveFile.Moves[i] };
                m.TypeID = moveFile.Moves[i].Type;
                m.TypeName = languageFile.GetTypeName(m.TypeID);
                m.Category = moveFile.Moves[i].Category.ToString();
                m.BasePower = moveFile.Moves[i].BasePower;
                m.BasePP = moveFile.Moves[i].BasePP;
                m.BaseAccuracy = moveFile.Moves[i].MoveAccuracy;
                m.PokemonLevelUp = new List<LevelPokemonReference>();
                m.PokemonTM = new List<PokemonReference>();
                m.PokemonEgg = new List<PokemonReference>();
                moves.Add(m);

                // Add to types
                types[m.TypeID].Moves.Add(new Models.EOS.MoveReference { ID = m.ID, Name = m.Name });
            }
            data.Moves = moves;

            // Read Pokemon
            var monsterFile = new MonsterMDFile();
            await monsterFile.OpenFile("/data/BALANCE/monster.md", rom);
            var pkms = new List<Models.EOS.Pokemon>();
            for (int i = 0; i < 600; i += 1)
            {
                var maleEntry = monsterFile.Entries[i];
                var femaleEntry = monsterFile.Entries.Count > i + 600 ? monsterFile.Entries[i + 600] : null;

                var entry = new Pokemon();
                entry.ID = i;// monsterFile.Entries[i].EntityID;
                entry.Name = languageFile.GetPokemonName(entry.ID % 600);
                entry.Data = data;
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
                            levelupMoves.Add(new Tuple<int, Move>(item.Item1, new Move { ID = item.Item2, Name = languageFile.GetMoveName(item.Item2), RawData = moveFile.Moves[item.Item2] }));

                            // Add Pokemon to move
                            var levelUp = moves[item.Item2].PokemonLevelUp.FirstOrDefault(x => x.ID == entry.ID);
                            if (levelUp == null)
                            {
                                moves[item.Item2].PokemonLevelUp.Add(new LevelPokemonReference(entry.ID, entry.Name, new List<string> { item.Item1.ToString() }));
                            }
                            else
                            {
                                levelUp.Levels.Add(item.Item1.ToString());
                            }

                        }
                    }
                    entry.LevelupMoves = levelupMoves;

                    var tmMoves = new List<Move>();
                    foreach (var item in moveFile.PokemonLearnsets[i].TMMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            tmMoves.Add(new Move { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonTM.Add(new PokemonReference(entry.ID, entry.Name));
                        }
                    }
                    entry.TMMoves = tmMoves;

                    var eggMoves = new List<Move>();
                    foreach (var item in moveFile.PokemonLearnsets[i].EggMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            eggMoves.Add(new Move { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonEgg.Add(new PokemonReference(entry.ID, entry.Name));
                        }

                    }
                    entry.EggMoves = eggMoves;
                }
                else
                {
                    entry.LevelupMoves = new List<Tuple<int, Move>>();
                    entry.TMMoves = new List<Move>();
                    entry.EggMoves = new List<Move>();
                }

                pkms.Add(entry);

                // Add to types
                types[maleEntry.MainType].Pokemon.Add(new PokemonReference(entry.ID, entry.Name));
                if (maleEntry.MainType != maleEntry.AltType)
                {
                    types[maleEntry.AltType].Pokemon.Add(new PokemonReference(entry.ID, entry.Name));
                }
            }
            data.Pokemon = pkms;

            return data;
        }

        static void BuildEOS(EosDataCollection data, string outputFilename)
        {
            var output = new List<Category>();

            // Pokemon
            var catPkm = new Category();
            catPkm.Name = "Eos-Pokemon";
            catPkm.Records = new List<Record>();
            foreach (var item in data.Pokemon)
            {
                catPkm.Records.Add(new Record
                {
                    Title = item.ID.ToString().PadLeft(3, '0') + " " + item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Pokemon.Details>(item),
                    InternalName = $"eos-pkm-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catPkm.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Pokemon.Index>(data.Pokemon),
                InternalName = $"eos-pkm-index"
            });
            output.Add(catPkm);

            // Moves
            var catMoves = new Category();
            catMoves.Name = "Eos-Moves";
            catMoves.Records = new List<Record>();
            foreach (var item in data.Moves)
            {
                catMoves.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Moves.Details>(item),
                    InternalName = $"eos-move-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catMoves.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Moves.Index>(data.Moves),
                InternalName = $"eos-move-index"
            });
            output.Add(catMoves);

            // Types
            var catTypes = new Category();
            catTypes.Name = "Eos-Types";
            catTypes.Records = new List<Record>();
            foreach (var item in data.Types)
            {
                catTypes.Records.Add(new Record
                {
                    Title = item.Name,
                    Content = BuildAndReturnTemplate<Views.EOS.Types.Details>(item),
                    InternalName = $"eos-type-" + item.ID,
                    Tags = new[] { item.Name }
                });
            }
            catTypes.Records.Add(new Record
            {
                Title = "Index",
                Content = BuildAndReturnTemplate<Views.EOS.Types.Index>(data.Types),
                InternalName = $"eos-type-index"
            });
            output.Add(catTypes);

            File.WriteAllText(outputFilename, JsonConvert.SerializeObject(output));
        }
    }
}
