using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class PokemonReference : IModelReference
    {
        public PokemonReference()
        {
        }

        public PokemonReference(Pokemon pkm)
        {
            ID = pkm.ID;
            Name = pkm.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
