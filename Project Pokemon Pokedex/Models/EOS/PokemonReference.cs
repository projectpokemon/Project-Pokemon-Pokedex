using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class PokemonReference
    {
        public PokemonReference(int id, string name)
        {
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
    }
}
