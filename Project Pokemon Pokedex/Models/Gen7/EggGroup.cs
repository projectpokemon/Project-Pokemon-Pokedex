using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class EggGroup
    {
        public string Name { get; set; }
        public List<Pokemon> SingleEggGroupPokemon { get; set; }
        public List<Pokemon> MultiEggGroupPokemon { get; set; }
    }
}
