using ProjectPokemon.Pokedex.Models.Games.Psmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.ViewModels.Psmd
{
    public class PsmdTypeDetailsViewModel
    {
        public PsmdTypeDetailsViewModel(PsmdPkmType type, PsmdDataCollection data)
        {
            ID = type.ID;
            Name = type.Name;
            var hex = ID.ToString("X").PadLeft(4, '0');
            IDHex = $"0x{hex}";

            PokemonWithType = new List<PsmdPokemonListItem>();
            PokemonWithType.AddRange(data.Pokemon.Where(x => x.Type1 == ID || x.Type2 == ID).Select(x => new PsmdPokemonListItem(x.ID, x.Name)));

            MovesWithType = new List<PsmdPokemonListItem>();
            MovesWithType.AddRange(data.Moves.Where(x => x.TypeID == ID).Select(x => new PsmdPokemonListItem(x.ID, x.Name)));
        }

        public int ID { get; set; }
        public string IDHex { get; set; }
        public string Name { get; set; }
        public List<PsmdPokemonListItem> PokemonWithType { get; set; }
        public List<PsmdPokemonListItem> MovesWithType { get; set; }
    }
}
