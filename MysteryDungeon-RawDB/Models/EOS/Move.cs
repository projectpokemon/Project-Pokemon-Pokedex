using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
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
    }
}
