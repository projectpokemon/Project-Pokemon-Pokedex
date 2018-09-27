using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosPokemonReference
    {
        public EosPokemonReference(EosDataCollection data, EosPokemon pokemon) : this(data, pokemon.ID, pokemon.Name)
        {
            Pokemon = pokemon;
        }

        public EosPokemonReference(EosDataCollection data, int id, string name)
        {
            Data = data;
            ID = id;
            Name = name;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string IDHexBigEndian
        {
            get
            {
                return "0x" + ID.ToString("X");
            }
        }

        public string IDHexLittleEndian
        {
            get
            {
                var hex = ID.ToString("X").PadLeft(4, '0');
                return string.Format("{0} {1}", hex.Substring(2, 2), hex.Substring(0, 2));
            }
        }

        private EosPokemon Pokemon { get; set; }
        private EosDataCollection Data { get; set; }

        public EosPokemon GetPokemon()
        {
            if (Pokemon != null)
            {
                return Pokemon;
            }
            else if (Data != null)
            {
                return Data.Pokemon.FirstOrDefault(p => p.ID == ID);
            }
            else
            {
                return null;
            }
        }
    }
}
