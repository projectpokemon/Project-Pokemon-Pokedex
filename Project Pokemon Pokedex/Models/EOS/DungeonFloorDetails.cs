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

        /// <summary>
        /// Determines whether the current instance of <see cref="DungeonFloorDetails"/> is equivalent to <paramref name="other"/>. Start and end floors are ignored.
        /// </summary>
        public bool IsEquivalentTo(DungeonFloorDetails other)
        {
            return this.FloorStructure == other.FloorStructure &&
                this.Music == other.Music &&
                this.InitialPokemonDensity == other.InitialPokemonDensity &&
                this.KeckleonShopPercentage == other.KeckleonShopPercentage &&
                this.MonsterHousePercentage == other.MonsterHousePercentage &&
                this.ItemDensity == other.ItemDensity &&
                this.TrapDensity == other.TrapDensity &&
                this.BuriedItemDensity == other.BuriedItemDensity &&
                this.WaterDensity == other.WaterDensity &&
                this.DarknessLevel == other.DarknessLevel &&
                this.CoinMax == other.CoinMax &&
                this.EnemyIQ == other.EnemyIQ &&
                this.Pokemon.SequenceEqual(other.Pokemon);
        }
    }
}
