using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.PSMD
{
    public class PokemonDetailsViewModel
    {
        public class MoveLevelUp
        {
            public int Level { get; set; }
            public int MoveID { get; set; }
            public string MoveName { get; set; }
        }
        public class ExpLevelUp
        {
            public int Level { get; set; }
            public uint Exp { get; set; }
            public int AddedHP { get; set; }
            public int AddedAttack { get; set; }
            public int AddedSpAttack { get; set; }
            public int AddedDefense { get; set; }
            public int AddedSpDefense { get; set; }
            public int AddedSpeed { get; set; }
            public int TotalHP { get; set; }
            public int TotalAttack { get; set; }
            public int TotalSpAttack { get; set; }
            public int TotalDefense { get; set; }
            public int TotalSpDefense { get; set; }
            public int TotalSpeed { get; set; }
        }

        public PokemonDetailsViewModel(Pokemon Pkm, PsmdDataCollection context)
        {
            ID = Pkm.ID;
            var hex = Pkm.ID.ToString("X").PadLeft(4, '0');
            IDHexBigEndian = $"0x{hex}";
            IDHexLittleEndian = $"{hex.Substring(2, 2)} {hex.Substring(0, 2)}";
            Name = Pkm.Name;
            DexNumber = Pkm.DexNumber;
            Category = Pkm.Category;
            ListNumber1 = Pkm.ListNumber1;
            ListNumber2 = Pkm.ListNumber2;
            BaseHP = Pkm.BaseHP;
            BaseAttack = Pkm.BaseAttack;
            BaseSpAttack = Pkm.BaseSpAttack;
            BaseDefense = Pkm.BaseDefense;
            BaseSpDefense = Pkm.BaseSpDefense;
            BaseSpeed = Pkm.BaseSpeed;
            EvolvesFromEntryID = Pkm.EvolvesFromEntry;
            EvolvesFromName = context.Pokemon.First(x => x.ID == EvolvesFromEntryID).Name;
            Ability1ID = Pkm.Ability1;
            Ability1 = context.Abilities.First(x => Pkm.Ability1 == x.ID).Name;
            Ability2ID = Pkm.Ability2;
            Ability2 = context.Abilities.First(x => Pkm.Ability2 == x.ID).Name;
            AbilityHidden = context.Abilities.First(x => Pkm.AbilityHidden == x.ID).Name;
            AbilityHiddenID = Pkm.AbilityHidden;
            Type1 = context.Types.First(x => Pkm.Type1 == x.ID).Name;
            Type2 = context.Types.First(x => Pkm.Type2 == x.ID).Name;
            IsMegaEvolution = (Pkm.IsMegaEvolution > 0);
            MinEvolveLevel = Pkm.MinEvolveLevel;

            MovesLevelUp = new List<MoveLevelUp>();
            MovesLevelUp.AddRange(from l in context.PokemonLevelUp
                                  join m in context.Moves on l.MoveID equals m.ID
                                  where l.PokemonID == Pkm.ID
                                  orderby l.Level, m.Name
                                  select new MoveLevelUp { Level = l.Level, MoveID = m.ID, MoveName = m.Name });

            StatLevelUp = new List<ExpLevelUp>();
            var attack = BaseAttack;
            var defense = BaseDefense;
            var spAttack = BaseSpAttack;
            var spDefense = BaseSpDefense;
            var speed = BaseSpeed;
            var hp = BaseHP;

            foreach (var item in context.Experience.Where(x => x.ExperienceTableNumber == Pkm.ExpTableNumber).OrderBy(x => x.Level))
            {
                var s = new ExpLevelUp();
                s.AddedAttack = item.AddedAttack;
                s.AddedDefense = item.AddedDefense;
                s.AddedHP = item.AddedHP;
                s.AddedSpAttack = item.AddedSpAttack;
                s.AddedSpDefense = item.AddedSpDefense;
                s.AddedSpeed = item.AddedSpeed;
                s.Exp = item.Exp;
                s.Level = item.Level;

                attack += item.AddedAttack;
                defense += item.AddedDefense;
                hp += item.AddedHP;
                spAttack += item.AddedSpAttack;
                spDefense += item.AddedSpDefense;
                speed += item.AddedSpeed;

                s.TotalAttack = attack;
                s.TotalDefense = defense;
                s.TotalHP = hp;
                s.TotalSpAttack = spAttack;
                s.TotalSpDefense = spDefense;
                s.TotalSpeed = speed;

                StatLevelUp.Add(s);
            }
        }


        public int ID { get; set; }

        public string IDHexBigEndian { get; set; }
        public string IDHexLittleEndian { get; set; }
        public string Name { get; set; }
        public int DexNumber { get; set; }
        public int Category { get; set; }
        public int ListNumber1 { get; set; }
        public int ListNumber2 { get; set; }
        public int BaseHP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseSpAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseSpDefense { get; set; }
        public int BaseSpeed { get; set; }
        public int EvolvesFromEntryID { get; set; }
        public string EvolvesFromName { get; set; }
        public int Ability1ID { get; set; }
        public int Ability2ID { get; set; }
        public int AbilityHiddenID { get; set; }

        public string Ability1 { get; set; }
        public string Ability2 { get; set; }
        public string AbilityHidden { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public bool IsMegaEvolution { get; set; }
        public byte MinEvolveLevel { get; set; }
        public List<MoveLevelUp> MovesLevelUp { get; set; }
        public List<ExpLevelUp> StatLevelUp { get; set; }

    }
}
