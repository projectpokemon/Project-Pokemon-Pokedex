using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7AbilityReference
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
