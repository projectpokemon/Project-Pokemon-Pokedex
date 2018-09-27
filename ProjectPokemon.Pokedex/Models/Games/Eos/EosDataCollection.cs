using SkyEditor.Core.IO;
using SkyEditor.ROMEditor.MysteryDungeon.Explorers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosDataCollection
    {
        public List<EosPokemon> Pokemon { get; set; }
        public List<EosMove> Moves { get; set; }
        public List<EosPkmType> Types { get; set; }

        /// <summary>
        /// Loads the EOS data from the given ROM
        /// </summary>
        public static async Task<EosDataCollection> LoadEosData(IIOProvider rom)
        {
            var data = new EosDataCollection();

            var languageFile = new LanguageString();
            await languageFile.OpenFile("/data/MESSAGE/text_e.str", rom);

            var moveFile = new waza_p();
            await moveFile.OpenFile("/data/BALANCE/waza_p.bin", rom);

            // Load Types
            var types = new List<EosPkmType>();
            for (int i = 0; i < 19; i++)
            {
                var t = new EosPkmType();
                t.ID = i;
                t.Name = languageFile.GetTypeName(i);
                t.Pokemon = new List<EosPokemonReference>();
                t.Moves = new List<EosMoveReference>();
                types.Add(t);
            }
            data.Types = types;

            // Read Move Data
            var moves = new List<EosMove>();
            for (int i = 0; i < moveFile.Moves.Count; i++)
            {
                var m = new EosMove { ID = i, Name = languageFile.GetMoveName(i), RawData = moveFile.Moves[i] };
                m.TypeID = moveFile.Moves[i].Type;
                m.TypeName = languageFile.GetTypeName(m.TypeID);
                m.Category = moveFile.Moves[i].Category.ToString();
                m.BasePower = moveFile.Moves[i].BasePower;
                m.BasePP = moveFile.Moves[i].BasePP;
                m.BaseAccuracy = moveFile.Moves[i].MoveAccuracy;
                m.PokemonLevelUp = new List<EosLevelPokemonReference>();
                m.PokemonTM = new List<EosPokemonReference>();
                m.PokemonEgg = new List<EosPokemonReference>();
                moves.Add(m);

                // Add to types
                types[m.TypeID].Moves.Add(new EosMoveReference { ID = m.ID, Name = m.Name });
            }
            data.Moves = moves;

            // Read Pokemon
            var monsterFile = new MonsterMDFile();
            await monsterFile.OpenFile("/data/BALANCE/monster.md", rom);
            var pkms = new List<EosPokemon>();
            for (int i = 0; i < 600; i += 1)
            {
                var maleEntry = monsterFile.Entries[i];
                var femaleEntry = monsterFile.Entries.Count > i + 600 ? monsterFile.Entries[i + 600] : null;

                var entry = new EosPokemon();
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
                    var e = new EosPokemon.GenderInfo();

                    e.BaseATK = maleEntry.BaseATK;
                    e.BaseDEF = maleEntry.BaseDEF;
                    e.BaseSPATK = maleEntry.BaseSPATK;
                    e.BaseSPDEF = maleEntry.BaseSPDEF;
                    e.BaseHP = maleEntry.BaseHP;

                    entry.Male = e;
                }

                if (femaleEntry != null)
                {
                    var e = new EosPokemon.GenderInfo();

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
                    var levelupMoves = new List<Tuple<int, EosMove>>();
                    foreach (var item in moveFile.PokemonLearnsets[i].LevelUpMoves)
                    {
                        if (moveFile.Moves.Count > item.Item2)
                        {
                            // Add move to Pokemon
                            levelupMoves.Add(new Tuple<int, EosMove>(item.Item1, new EosMove { ID = item.Item2, Name = languageFile.GetMoveName(item.Item2), RawData = moveFile.Moves[item.Item2] }));

                            // Add Pokemon to move
                            var levelUp = moves[item.Item2].PokemonLevelUp.FirstOrDefault(x => x.ID == entry.ID);
                            if (levelUp == null)
                            {
                                moves[item.Item2].PokemonLevelUp.Add(new EosLevelPokemonReference(data, entry.ID, entry.Name, new List<string> { item.Item1.ToString() }));
                            }
                            else
                            {
                                levelUp.Levels.Add(item.Item1.ToString());
                            }

                        }
                    }
                    entry.LevelupMoves = levelupMoves;

                    var tmMoves = new List<EosMove>();
                    foreach (var item in moveFile.PokemonLearnsets[i].TMMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            tmMoves.Add(new EosMove { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonTM.Add(new EosPokemonReference(data, entry.ID, entry.Name));
                        }
                    }
                    entry.TMMoves = tmMoves;

                    var eggMoves = new List<EosMove>();
                    foreach (var item in moveFile.PokemonLearnsets[i].EggMoves)
                    {
                        if (moveFile.Moves.Count > item)
                        {
                            // Add move to Pokemon
                            eggMoves.Add(new EosMove { ID = item, Name = languageFile.GetMoveName(item), RawData = moveFile.Moves[item] });

                            // Add Pokemon to move
                            moves[item].PokemonEgg.Add(new EosPokemonReference(data, entry.ID, entry.Name));
                        }

                    }
                    entry.EggMoves = eggMoves;
                }
                else
                {
                    entry.LevelupMoves = new List<Tuple<int, EosMove>>();
                    entry.TMMoves = new List<EosMove>();
                    entry.EggMoves = new List<EosMove>();
                }

                pkms.Add(entry);

                // Add to types
                types[maleEntry.MainType].Pokemon.Add(new EosPokemonReference(data, entry.ID, entry.Name));
                if (maleEntry.MainType != maleEntry.AltType)
                {
                    types[maleEntry.AltType].Pokemon.Add(new EosPokemonReference(data, entry.ID, entry.Name));
                }
            }
            data.Pokemon = pkms;

            return data;
        }
    }
}
