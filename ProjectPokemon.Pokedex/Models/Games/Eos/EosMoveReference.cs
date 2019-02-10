using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Eos
{
    public class EosMoveReference
    {
        public EosMoveReference(EosDataCollection data, EosMove move)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Move = move ?? throw new ArgumentNullException(nameof(move));
        }
        public EosMoveReference(EosDataCollection data, int id, string name)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            ID = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int ID { get; set; }
        public string Name { get; set; }
        private EosMove Move { get; set; }
        private EosDataCollection Data { get; set; }

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
