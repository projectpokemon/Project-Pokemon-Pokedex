using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class PkmType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<PokemonReference> Pokemon { get; set; }

        public List<MoveReference> Moves { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
