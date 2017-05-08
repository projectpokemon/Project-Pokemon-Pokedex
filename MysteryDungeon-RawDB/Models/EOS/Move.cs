using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
{
    public class Move
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public SkyEditor.ROMEditor.MysteryDungeon.Explorers.waza_p.MoveData RawData { get; set; }
    }
}
