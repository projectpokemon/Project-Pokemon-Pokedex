using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class EvolutionMethod
    {
        public PokemonReference TargetPokemon { get; set; }
        public int Level { get; set; }
        public int Form { get; set; }

        /// <summary>
        /// A user-friendly string representing how the Pokemon evolves. Placeholder {0} refers to either <see cref="ParameterReference"/> or <see cref="ParameterString"/>.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A reference to an item, move, species, or type. If null, refer to <see cref="ParameterString"/>.
        /// </summary>
        public IModelReference ParameterReference { get; set; }

        /// <summary>
        /// The user-friendly string representing the evolution parameter. Refer to <see cref="ParameterReference"/> first if it is not null.
        /// </summary>
        public string ParameterString { get; set; }
    }
}
