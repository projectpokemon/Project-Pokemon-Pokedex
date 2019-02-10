using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7LevelupPokemonReference : Gen7PokemonReference
    {
        public Gen7LevelupPokemonReference(int id, string name, Gen7DataCollection data) : base(id, name, data)
        {
        }

        public Gen7LevelupPokemonReference(Gen7Pokemon pkm) : base(pkm)
        {
        }

        public List<int> Levels { get; set; }
    }
}
