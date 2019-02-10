using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7PokemonReference : IModelReference
    {
        public Gen7PokemonReference(int id, string name, Gen7DataCollection data)
        {
            ID = id;
            Name = name;
            _data = data;
        }

        public Gen7PokemonReference(Gen7Pokemon pkm)
        {
            ID = pkm.ID;
            Name = pkm.Name;
            _pokemon = pkm;
        }

        private Gen7Pokemon Pokemon
        {
            get
            {
                if (_pokemon != null)
                {
                    return _pokemon;
                }
                else if (_data != null)
                {
                    return _data.Pokemon.FirstOrDefault(x => x.ID == ID);
                }
                else
                {
                    return null;
                }
            }
        }
        private Gen7Pokemon _pokemon;

        private Gen7DataCollection _data;

        public int ID { get; set; }
        public string Name { get; set; }
        public string PokespriteHtml
        {
            get
            {
                return Pokemon?.PokespriteHtml ?? "";
            }
        }

        public Gen7TypeReference Type1
        {
            get
            {
                return Pokemon?.Type1 ?? throw new NullReferenceException("PokemonReference.Pokemon is null");
            }
        }

        public Gen7TypeReference Type2
        {
            get
            {
                return Pokemon?.Type2 ?? throw new NullReferenceException("PokemonReference.Pokemon is null");
            }
        }

        public Gen7AbilityReference Ability1
        {
            get
            {
                return Pokemon?.Ability1 ?? throw new NullReferenceException("PokemonReference.Pokemon is null");
            }
        }

        public Gen7AbilityReference Ability2
        {
            get
            {
                return Pokemon?.Ability2 ?? throw new NullReferenceException("PokemonReference.Pokemon is null");
            }
        }

        public Gen7AbilityReference AbilityHidden
        {
            get
            {
                return Pokemon?.AbilityHidden ?? throw new NullReferenceException("PokemonReference.Pokemon is null");
            }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
