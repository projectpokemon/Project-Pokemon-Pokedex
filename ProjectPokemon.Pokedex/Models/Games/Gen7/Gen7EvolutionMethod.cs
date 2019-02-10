using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7EvolutionMethod
    {
        public Gen7PokemonReference TargetPokemon { get; set; }
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

        public override bool Equals(object obj)
        {
            var other = obj as Gen7EvolutionMethod;
            return other != null && TargetPokemon.ID == other.TargetPokemon.ID && Level == other.Level && Form == other.Form && Method == other.Method && ParameterReference?.ID == other.ParameterReference?.ID && ParameterString == other.ParameterString;
        }

        public override int GetHashCode()
        {
            return TargetPokemon.ID ^ Level ^ Form ^ Method.GetHashCode() ^ (ParameterReference?.ID ?? 0) ^ ParameterString.GetHashCode();
        }
    }
}
