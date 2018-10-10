using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Psmd
{
    public class PsmdMoveMultiHit
    {
        public int ID { get; set; }
        public bool RepeatUntilMiss { get; set; }
        public int HitCountMinimum { get; set; }
        public int HitCountMaximum { get; set; }
        public int HitChance2 { get; set; }
        public int HitChance3 { get; set; }
        public int HitChance4 { get; set; }
        public int HitChance5 { get; set; }
    }
}
