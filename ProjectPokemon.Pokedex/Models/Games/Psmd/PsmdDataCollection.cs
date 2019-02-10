using SkyEditor.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Psmd
{
    public class PsmdDataCollection
    {
        public List<PsmdAbility> Abilities { get; set; }
        public List<PsmdPkmType> Types { get; set; }
        public List<PsmdMove> Moves { get; set; }
        public List<PsmdMoveMultiHit> MoveMultiHit { get; set; }
        public List<PsmdPokemon> Pokemon { get; set; }
        public List<PsmdPokemonLevelUp> PokemonLevelUp { get; set; }
        public List<PsmdExperienceLevel> Experience { get; set; }

        public static async Task<PsmdDataCollection> LoadPsmdData(IIOProvider rom)
        {
            var data = new PsmdDataCollection();

            // Load abilities
            var abilities = new List<PsmdAbility>();
            var abilityNames = File.ReadAllLines("App_Data/PSMD/alist.txt");
            for (int i = 0; i < abilityNames.Length; i++)
            {
                abilities.Add(new PsmdAbility
                {
                    ID = i,
                    Name = abilityNames[i]
                });
            }
            data.Abilities = abilities;

            // Load types
            var types = new List<PsmdPkmType>();
            var typeNames = File.ReadAllLines("App_Data/PSMD/tlist.txt");
            for (int i = 0; i < typeNames.Length; i++)
            {
                types.Add(new PsmdPkmType
                {
                    ID = i,
                    Name = typeNames[i]
                });
            }
            data.Types = types;


            // Load moves
            var wazaFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.WazaDataInfo();
            await wazaFile.OpenFile("/RomFS/pokemon/waza_data_info.bin", rom);

            var actFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.ActDataInfo();
            await actFile.OpenFile("/RomFS/dungeon/act_data_info.bin", rom);

            var actHitFile = new SkyEditor.ROMEditor.MysteryDungeon.PSMD.ActHitCountTableDataInfo();
            await actHitFile.OpenFile("/RomFS/dungeon/act_hit_count_table_data_info.bin", rom);

            var moves = new List<PsmdMove>();
            var moveNames = File.ReadAllLines("App_Data/PSMD/mlist.txt");
            for (int i = 0; i < moveNames.Length; i++)
            {
                var m = new PsmdMove();
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
            var multi = new List<PsmdMoveMultiHit>();
            for (int i = 0; i < actHitFile.Entries.Count; i++)
            {
                var m = new PsmdMoveMultiHit();
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
            await pokemonFile.OpenFile("/RomFS/pokemon/pokemon_data_info.bin", rom);

            var pkms = new List<PsmdPokemon>();
            var pkmNames = File.ReadAllLines("App_Data/PSMD/plist.txt");
            for (int i = 0; i < pkmNames.Length; i++)
            {
                var pkm = new PsmdPokemon
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
            var levels = new List<PsmdPokemonLevelUp>();
            for (int i = 0; i < pokemonFile.Entries.Count; i++)
            {
                var pkm = pokemonFile.Entries[i];
                for (int j = 0; j < pkm.MoveLevels.Length; j++)
                {
                    if (pkm.Moves[j] != 0)
                    {
                        levels.Add(new PsmdPokemonLevelUp
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
            await expTables.OpenFile("/RomFS/pokemon/experience.bin", rom);
            var exps = new List<PsmdExperienceLevel>();
            for (int expTableNum = 0; expTableNum < expTables.Entries.Count; expTableNum++)
            {
                for (int level = 0; level < expTables.Entries[expTableNum].Count; level++)
                {
                    var e = expTables.Entries[expTableNum][level];
                    exps.Add(new PsmdExperienceLevel
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
    }
}
