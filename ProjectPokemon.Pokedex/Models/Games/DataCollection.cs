using ProjectPokemon.Pokedex.Models.Games.Eos;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using ProjectPokemon.Pokedex.Models.Games.Psmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games
{
    public class DataCollection
    {
        public EosDataCollection EosData { get; set; }
        public PsmdDataCollection PsmdData { get; set; }
        public Gen7DataCollection SMData { get; set; }
        public Gen7DataCollection UltraSMData { get; set; }
    }
}
