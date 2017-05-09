using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
{
    public class LevelPokemonReference : PokemonReference
    {
        public List<string> Levels { get; set; }
    }
}
