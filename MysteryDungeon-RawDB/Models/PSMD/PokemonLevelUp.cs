using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.PSMD
{
    public class PokemonLevelUp
    {
        public int PokemonID { get; set; }
        public int Level { get; set; }
        public int MoveID { get; set; }
    }
}
