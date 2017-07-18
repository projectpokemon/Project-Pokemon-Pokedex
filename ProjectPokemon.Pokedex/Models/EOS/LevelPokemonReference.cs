using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class LevelPokemonReference : PokemonReference
    {
        public LevelPokemonReference(int id, string name, List<string> levels) : base(id, name)
        {
            Levels = levels;
        }

        public List<string> Levels { get; set; }
    }
}
