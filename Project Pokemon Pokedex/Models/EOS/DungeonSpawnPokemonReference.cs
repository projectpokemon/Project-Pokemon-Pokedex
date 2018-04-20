using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class DungeonSpawnPokemonReference : PokemonReference
    {
        public DungeonSpawnPokemonReference(int id, string name, int level, int probability1, int probability2, int unknown, EosDataCollection data) : base(id, name, data)
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

        public override bool Equals(object obj)
        {
            if (obj is DungeonSpawnPokemonReference other)
            {
                return this.ID == other.ID &&
                    this.Name == other.Name &&
                    this.Level == other.Level &&
                    this.Probability1 == other.Probability1 &&
                    this.Probability2 == other.Probability2 &&
                    this.Unknown == other.Unknown;
            }
            else
            {
                return false;
            }
        }
    }
}
