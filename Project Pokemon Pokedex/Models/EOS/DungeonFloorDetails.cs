using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class DungeonFloorDetails
    {
        /// <summary>
        /// The first floor this details instance represents
        /// </summary>
        public int StartFloor { get; set; }

        /// <summary>
        /// The last floor this details instance represents
        /// </summary>
        public int EndFloor { get; set; }

        public string FloorStructure { get; set; }
        public string Music { get; set; }
        public int InitialPokemonDensity { get; set; }
        public int KeckleonShopPercentage { get; set; }
        public int MonsterHousePercentage { get; set; }
        public int ItemDensity { get; set; }
        public int TrapDensity { get; set; }
        public int BuriedItemDensity { get; set; }
        public int WaterDensity { get; set; }
        public int DarknessLevel { get; set; }
        public int CoinMax { get; set; } // Steps of 40
        public int EnemyIQ { get; set; }

        public List<DungeonSpawnPokemonReference> Pokemon { get; set; }
    }
}
