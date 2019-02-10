using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7LevelupMoveReference : Gen7MoveReference
    {
        public Gen7LevelupMoveReference(int id, string name, Gen7DataCollection data) : base(id, name, data)
        {
        }

        public Gen7LevelupMoveReference(int id, string name, int level, Gen7DataCollection data) : this(id, name, data)
        {
            this.Level = level;
        }

        public Gen7LevelupMoveReference(Gen7Move move, Gen7DataCollection data) : base(move, data)
        {
        }

        public int Level { get; set; }
    }
}
