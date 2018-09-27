using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosPkmType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<EosPokemonReference> Pokemon { get; set; }
        public List<EosMoveReference> Moves { get; set; }
    }
}
