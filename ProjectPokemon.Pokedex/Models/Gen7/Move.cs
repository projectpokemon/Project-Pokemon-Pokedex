using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class Move
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TypeReference Type { get; set; }
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

        /// <summary>
        /// Unknown. PK3DS says "Something to deal with skipImmunity"
        /// </summary>
        public int UnknownB { get; set; }

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

        /// <summary>
        /// Unknown (Bitflag Related for stuff like Contact and Extra Move Effects)
        /// </summary>
        public int Unknown20 { get; set; }

        /// <summary>
        /// Unknown (Bitflag Related for stuff like Contact and Extra Move Effects)
        /// </summary>
        public int Unknown21 { get; set; }

        public List<LevelupPokemonReference> PokemonThroughLevelUp { get; set; }
        public List<PokemonReference> PokemonThroughTM { get; set; }
        public List<PokemonReference> PokemonThroughEgg { get; set; }
        public List<PokemonReference> PokemonThroughTutor { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
