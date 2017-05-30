using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.PSMD
{
    public class Pokemon
    {
        public int ID { get; set; }
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
        public int EvolvesFromEntry { get; set; }
        public int ExpTableNumber { get; set; }
        public int Ability1 { get; set; }
        public int Ability2 { get; set; }
        public int AbilityHidden { get; set; }
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public byte IsMegaEvolution { get; set; }
        public byte MinEvolveLevel { get; set; }
    }
}
