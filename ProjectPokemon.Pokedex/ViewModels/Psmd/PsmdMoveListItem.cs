using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.ViewModels.Psmd
{
    public class PsmdMoveListItem
    {
        public PsmdMoveListItem(int id, string name)
        {
            ID = id;
            Name = name;
            var hex = id.ToString("X").PadLeft(4, '0');
            IDHex = $"0x{hex}";
        }

        public int ID { get; set; }
        public string IDHex { get; set; }
        public string Name { get; set; }
    }
}
