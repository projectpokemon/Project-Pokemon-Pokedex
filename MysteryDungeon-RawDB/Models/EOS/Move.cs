using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
{
    public class Move
    {
        public class PokemonReference
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class LevelPokemonReference : PokemonReference
        {
            public int Level { get; set; }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public SkyEditor.ROMEditor.MysteryDungeon.Explorers.waza_p.MoveData RawData { get; set; }

        public List<LevelPokemonReference> PokemonLevelup { get; set; }
        public List<PokemonReference> PokemonTM { get; set; }
        public List<PokemonReference> PokemonEgg { get; set; }
    }
}
