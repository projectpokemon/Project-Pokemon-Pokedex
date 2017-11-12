using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class SMDataCollection
    {
        public List<Pokemon> Pokemon { get; set; }
        public List<Move> Moves { get; set; }
        public List<PkmType> Types { get; set; }
        public string[][] AltFormStrings { get; set; }
        public bool IsUltra { get; set; }

        public TypeEffectivenessChart TypeEffectiveness { get; set; }
    }
}
