using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class LevelupPokemonReference : PokemonReference
    {
        public List<int> Levels { get; set; }
    }
}
