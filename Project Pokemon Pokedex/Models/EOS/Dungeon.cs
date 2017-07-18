using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class Dungeon
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DungeonFloorDetails> Floors { get; set; }
    }
}
