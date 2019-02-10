using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7Move
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Gen7TypeReference Type { get; set; }
        public string Qualities { get; set; }
        public string Category { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public int PP { get; set; }
        public int Priority { get; set; }
        public int HitMin { get; set; }
        public int HitMax { get; set; }
        public string Inflict { get; set; }

        /// <summary>
        /// An integer representing the inflict percent chance
        /// </summary>
        public int InflictChance { get; set; }

        public int TurnMin { get; set; }
        public int TurnMax { get; set; }
        public int CritStage { get; set; }
        public int Flinch { get; set; }
        public int Effect { get; set; }
        public int Recoil { get; set; }
        public string Heal { get; set; }

        public string Targeting { get; set; }
        public string Stat1 { get; set; }
        public string Stat2 { get; set; }
        public string Stat3 { get; set; }
        public int Stat1Num { get; set; }
        public int Stat2Num { get; set; }
        public int Stat3Num { get; set; }
        public int Stat1Percent { get; set; }
        public int Stat2Percent { get; set; }
        public int Stat3Percent { get; set; }

        public List<Gen7LevelupPokemonReference> PokemonThroughLevelUp { get; set; }
        public List<Gen7PokemonReference> PokemonThroughTM { get; set; }
        public List<Gen7PokemonReference> PokemonThroughEgg { get; set; }
        public List<Gen7PokemonReference> PokemonThroughTutor { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
