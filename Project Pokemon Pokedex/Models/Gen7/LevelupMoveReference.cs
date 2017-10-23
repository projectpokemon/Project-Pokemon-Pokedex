using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class LevelupMoveReference : MoveReference
    {
        public LevelupMoveReference(int id, string name, SMDataCollection data) : base(id, name, data)
        {
        }

        public LevelupMoveReference(int id, string name, int level, SMDataCollection data) : this(id, name, data)
        {
            this.Level = level;
        }

        public LevelupMoveReference(Move move, SMDataCollection data) : base(move, data)
        {
        }

        public int Level { get; set; }
    }
}
