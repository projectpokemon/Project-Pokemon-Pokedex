using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosMoveReference
    {
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
