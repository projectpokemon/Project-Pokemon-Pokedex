using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.PSMD
{
    public class BasicPokemonListItem
    {
        public BasicPokemonListItem(int id, string name)
        {
            ID = id;
            Name = name;
            var hex = id.ToString("X").PadLeft(4, '0');
            IDHexBigEndian = $"0x{hex}";
            IDHexLittleEndian = $"{hex.Substring(2, 2)} {hex.Substring(0, 2)}";
        }

        public int ID { get; set; }
        public string IDHexBigEndian { get; set; }
        public string IDHexLittleEndian { get; set; }
        public string Name { get; set; }
    }
}
