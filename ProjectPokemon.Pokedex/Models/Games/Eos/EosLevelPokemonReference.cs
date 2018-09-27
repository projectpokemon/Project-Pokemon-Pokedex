using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosLevelPokemonReference : EosPokemonReference
    {
        public EosLevelPokemonReference(EosDataCollection data, int id, string name, List<string> levels) : base(data, id, name)
        {
            Levels = levels;
        }

        public List<string> Levels { get; set; }
    }
}
