using ProjectPokemon.Pokedex.Models.EOS;
using ProjectPokemon.Pokedex.Models.Gen7;
using ProjectPokemon.Pokedex.Models.PSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models
{
    public class DataCollection
    {
        public EosDataCollection EosData { get; set; }
        public PsmdDataCollection PsmdData { get; set; }
        public SMDataCollection SMData { get; set; }
        public SMDataCollection UltraSMData { get; set; }
    }
}
