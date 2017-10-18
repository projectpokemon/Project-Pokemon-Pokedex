using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class PokemonReference : IModelReference
    {
        public PokemonReference(int id, string name, SMDataCollection data)
        {
            ID = id;
            Name = name;
            _data = data;
        }

        public PokemonReference(Pokemon pkm)
        {
            ID = pkm.ID;
            Name = pkm.Name;
            _pokemon = pkm;
        }

        private Pokemon Pokemon
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
        private Pokemon _pokemon;
        private SMDataCollection _data;

        public int ID { get; set; }
        public string Name { get; set; }
        public string PokespriteHtml
        {
            get
            {
                return Pokemon?.PokespriteHtml ?? "";
            }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
