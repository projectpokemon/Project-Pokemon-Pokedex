using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.PSMD
{
    public class PsmdDataCollection
    {
        public List<Ability> Abilities { get; set; }
        public List<PkmType> Types { get; set; }
        public List<Move> Moves { get; set; }
        public List<MoveMultiHit> MoveMultiHit { get; set; }
        public List<Pokemon> Pokemon { get; set; }
        public List<PokemonLevelUp> PokemonLevelUp { get; set; }
        public List<ExperienceLevel> Experience { get; set; }
    }
}
