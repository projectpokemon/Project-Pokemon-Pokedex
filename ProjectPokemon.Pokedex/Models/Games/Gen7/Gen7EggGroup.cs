using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7EggGroup
    {
        public string Name { get; set; }
        public List<Gen7Pokemon> SingleEggGroupPokemon { get; set; }
        public List<Gen7Pokemon> MultiEggGroupPokemon { get; set; }
    }
}
