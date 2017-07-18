using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.PSMD
{
    public class Move
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int EffectRate { get; set; }
        public int HPBellyChangeValue { get; set; }
        public bool TrapFlag { get; set; }
        public int StatusChange { get; set; }
        public int StatChangeIndex { get; set; }
        public int TypeChange { get; set; }
        public int TerrainChange { get; set; }
        public int BaseAccuracy { get; set; }
        public int MaxAccuracy { get; set; }
        public int SizeTypeMove { get; set; }
        public int TypeID { get; set; }
        public int Attribute { get; set; }
        public int BaseDamage { get; set; }
        public int MaxDamage { get; set; }
        public int BasePP { get; set; }
        public int MaxPP { get; set; }
        public bool CutsCorners { get; set; }
        public bool MoreTimeToAttack { get; set; }
        public int TilesCount { get; set; }
        public int Range { get; set; }
        public int Target { get; set; }
        public bool PiercingAttack { get; set; }
        public bool SleepAttack { get; set; }
        public bool FaintAttack { get; set; }
        public bool NearbyDamage { get; set; }
        public int HitCounterIndex { get; set; }
    }
}
