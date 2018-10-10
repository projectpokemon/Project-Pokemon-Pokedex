using ProjectPokemon.Pokedex.Models.Games.Psmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.ViewModels.Psmd
{
    public class PsmdAbilityDetailsViewModel
    {
        public PsmdAbilityDetailsViewModel(PsmdAbility ability, PsmdDataCollection data)
        {
            ID = ability.ID;
            Name = ability.Name;
            var hex = ID.ToString("X").PadLeft(4, '0');
            IDHexBigEndian = $"0x{hex}";
            IDHexLittleEndian = $"{hex.Substring(2, 2)} {hex.Substring(0, 2)}";

            PokemonWithAbility1 = new List<PsmdPokemonListItem>();
            PokemonWithAbility2 = new List<PsmdPokemonListItem>();
            PokemonWithAbilityHidden = new List<PsmdPokemonListItem>();

            foreach (var item in from p in data.Pokemon
                                 where p.Ability1 == ID || p.Ability2 == ID || p.AbilityHidden == ID
                                 select new { p.ID, p.Name, p.Ability1, p.Ability2, p.AbilityHidden })
            {
                var p = new PsmdPokemonListItem(item.ID, item.Name);
                if (item.Ability1 == ID)
                {
                    PokemonWithAbility1.Add(p);
                }
                if (item.Ability2 == ID)
                {
                    PokemonWithAbility2.Add(p);
                }
                if (item.AbilityHidden == ID)
                {
                    PokemonWithAbilityHidden.Add(p);
                }
            }
        }

        public int ID { get; set; }
        public string IDHexBigEndian { get; set; }
        public string IDHexLittleEndian { get; set; }
        public string Name { get; set; }
        public List<PsmdPokemonListItem> PokemonWithAbility1 { get; set; }
        public List<PsmdPokemonListItem> PokemonWithAbility2 { get; set; }
        public List<PsmdPokemonListItem> PokemonWithAbilityHidden { get; set; }
    }
}
