using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class DungeonSpawnPokemonReference : PokemonReference
    {
        public DungeonSpawnPokemonReference(int id, string name, int level, int probability1, int probability2, int unknown) : base(id, name)
        {
            Level = level;
            Probability1 = probability1;
            Probability2 = probability2;
            Unknown = unknown;
        }

        public int Level { get; set; }
        public int Probability1 { get; set; }
        public int Probability2 { get; set; }
        public int Unknown { get; set; }
    }
}
