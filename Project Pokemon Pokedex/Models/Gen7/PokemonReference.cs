using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class PokemonReference : IModelReference
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
