using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
{
    public class PkmType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<PokemonReference> Pokemon { get; set; }
        public List<MoveReference> Moves { get; set; }
    }
}
