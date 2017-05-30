using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class Move
    {        
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string Category { get; set; }
        public int BasePower { get; set; }
        public int BasePP { get; set; }
        public int BaseAccuracy { get; set; }

        public SkyEditor.ROMEditor.MysteryDungeon.Explorers.waza_p.MoveData RawData { get; set; }

        public List<LevelPokemonReference> PokemonLevelUp { get; set; }
        public List<PokemonReference> PokemonTM { get; set; }
        public List<PokemonReference> PokemonEgg { get; set; }

        public string GetIDHexBigEndian()
        {
            return "0x" + ID.ToString("X");
        }

        public string GetIDHexLittleEndian()
        {
            var hex = ID.ToString("X").PadLeft(4, '0');
            return string.Format("{0} {1}", hex.Substring(2, 2), hex.Substring(0, 2));
        }
    }
}
