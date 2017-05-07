using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.PSMD
{
    public class MoveDetailsViewModel
    {
        public class PkmLevelUp
        {
            public PkmLevelUp(int id, string name, IEnumerable<int> levels)
            {
                ID = id;
                var hex = id.ToString("X").PadLeft(4, '0');
                IDHexBigEndian = $"0x{hex}";
                IDHexLittleEndian = $"{hex.Substring(2, 2)} {hex.Substring(0, 2)}";
                Name = name;

                Levels = new List<int>();
                Levels.AddRange(levels);
            }

            public int ID { get; set; }

            public string IDHexBigEndian { get; set; }
            public string IDHexLittleEndian { get; set; }
            public string Name { get; set; }
            public List<int> Levels { get; set; }
        }

        public MoveDetailsViewModel(Move move, PsmdDataCollection data)
        {
            ID = move.ID;
            var hex = move.ID.ToString("X").PadLeft(4, '0');
            IDHexBigEndian = $"0x{hex}";
            IDHexLittleEndian = $"{hex.Substring(2, 2)} {hex.Substring(0, 2)}";
            Name = move.Name;

            PokemonLevelUp = new List<PkmLevelUp>();
            foreach (var item in from l in data.PokemonLevelUp
                                 join p in data.Pokemon on l.PokemonID equals p.ID
                                 where l.MoveID == move.ID
                                 orderby l.PokemonID, l.Level
                                 select new { PokemonID = l.PokemonID, PokemonName = p.Name, Level = l.Level})
            {
                var levelup = PokemonLevelUp.Where(x => x.ID == item.PokemonID).FirstOrDefault();
                if (levelup == null)
                {
                    PokemonLevelUp.Add(new PkmLevelUp(item.PokemonID, item.PokemonName, new int[] { item.Level }));
                }
                else
                {
                    if (!levelup.Levels.Contains(item.Level))
                    {
                        levelup.Levels.Add(item.Level);
                    }
                }
            }

            MultiHitData = data.MoveMultiHit.First(x => x.ID == move.HitCounterIndex);

            this.EffectRate = move.EffectRate;
            this.HPBellyChangeValue = move.HPBellyChangeValue;
            this.TrapFlag = move.TrapFlag;
            this.StatusChange = move.StatusChange;
            this.StatChangeIndex = move.StatChangeIndex;
            this.TypeChange = move.TypeChange;
            this.TerrainChange = move.TerrainChange;
            this.BaseAccuracy = move.BaseAccuracy;
            this.MaxAccuracy = move.MaxAccuracy;
            this.SizeTypeMove = move.SizeTypeMove;
            this.TypeID = move.TypeID;
            this.TypeName = data.Types.First(x => x.ID == move.TypeID).Name;
            this.Attribute = move.Attribute;
            this.BaseDamage = move.BaseDamage;
            this.MaxDamage = move.MaxDamage;
            this.BasePP = move.BasePP;
            this.MaxPP = move.MaxPP;
            this.CutsCorners = move.CutsCorners;
            this.MoreTimeToAttack = move.MoreTimeToAttack;
            this.TilesCount = move.TilesCount;
            this.Range = move.Range;
            this.Target = move.Target;
            this.PiercingAttack = move.PiercingAttack;
            this.SleepAttack = move.SleepAttack;
            this.FaintAttack = move.FaintAttack;
            this.NearbyDamage = move.NearbyDamage;
        }

        public int ID { get; set; }

        public string IDHexBigEndian { get; set; }
        public string IDHexLittleEndian { get; set; }
        public string Name { get; set; }
        public List<PkmLevelUp> PokemonLevelUp { get; set; }
        public MoveMultiHit MultiHitData { get; set; }
        public int EffectRate { get; set; }
        public int HPBellyChangeValue { get; set; }
        public bool TrapFlag { get; set; }
        public int StatusChange { get; set; }
        public int StatChangeIndex { get; set; }
        public int TypeChange { get; set; }
        public int TerrainChange { get; set; }
        public int BaseAccuracy { get; set; }
        public int MaxAccuracy { get; set; }
        public int SizeTypeMove { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int Attribute { get; set; }
        public int BaseDamage { get; set; }
        public int MaxDamage { get; set; }
        public int BasePP { get; set; }
        public int MaxPP { get; set; }
        public bool CutsCorners { get; set; }
        public bool MoreTimeToAttack { get; set; }
        public int TilesCount { get; set; }
        public int Range { get; set; }
        public int Target { get; set; }
        public bool PiercingAttack { get; set; }
        public bool SleepAttack { get; set; }
        public bool FaintAttack { get; set; }
        public bool NearbyDamage { get; set; }
    }
}
