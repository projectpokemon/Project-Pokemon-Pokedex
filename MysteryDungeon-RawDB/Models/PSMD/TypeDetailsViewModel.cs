using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.PSMD
{
    public class TypeDetailsViewModel
    {
        public TypeDetailsViewModel(PkmType type, PsmdDataCollection data)
        {
            ID = type.ID;
            Name = type.Name;
            var hex = ID.ToString("X").PadLeft(4, '0');
            IDHex = $"0x{hex}";

            PokemonWithType = new List<BasicPokemonListItem>();
            PokemonWithType.AddRange(data.Pokemon.Where(x => x.Type1 == ID || x.Type2 == ID).Select(x => new BasicPokemonListItem(x.ID, x.Name)));

            MovesWithType = new List<BasicMoveListItem>();
            MovesWithType.AddRange(data.Moves.Where(x => x.TypeID == ID).Select(x => new BasicMoveListItem(x.ID, x.Name)));
        }

        public int ID { get; set; }
        public string IDHex { get; set; }
        public string Name { get; set; }
        public List<BasicPokemonListItem> PokemonWithType { get; set; }
        public List<BasicMoveListItem> MovesWithType { get; set; }
    }
}
