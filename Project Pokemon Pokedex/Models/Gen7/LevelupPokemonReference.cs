using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class LevelupPokemonReference : PokemonReference
    {
        public LevelupPokemonReference(int id, string name, SMDataCollection data) : base(id, name, data)
        {
        }

        public LevelupPokemonReference(Pokemon pkm) : base(pkm)
        { 
        }

        public List<int> Levels { get; set; }
    }
}
