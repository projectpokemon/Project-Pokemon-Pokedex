using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7PkmType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Gen7PokemonReference> Pokemon { get; set; }

        public List<Gen7MoveReference> Moves { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
