using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class Pokemon
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int BaseHP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseSpeed { get; set; }
        public int BaseSpAttack { get; set; }
        public int BaseSpDefense { get; set; }
        public int HPEVYield { get; set; }
        public int AttackEVYield { get; set; }
        public int DefenseEVYield { get; set; }
        public int SpeedEVYield { get; set; }
        public int SpAttackEVYield { get; set; }
        public int SpDefenseEVYield { get; set; }

        public TypeReference Type1 { get; set; }
        public TypeReference Type2 { get; set; }

        public int CatchRate { get; set; }
        public int EvoStage { get; set; }

        /// <summary>
        /// Possible hold item with a 50% chance of being held
        /// </summary>
        public ItemReference HeldItem1 { get; set; }

        /// <summary>
        /// Possible hold item with a 5% chance of being held
        /// </summary>
        public ItemReference HeldItem2 { get; set; }

        /// <summary>
        /// Possible hold item with a 1% chance of being held
        /// </summary>
        public ItemReference HeldItem3 { get; set; }

        public int Gender { get; set; }
        public int HatchCycles { get; set; }
        public int BaseFriendship { get; set; }

        public string ExpGrowth { get; set; }

        public string EggGroup1 { get; set; }
        public string EggGroup2 { get; set; }
        
        public AbilityReference Ability1 { get; set; }
        public AbilityReference Ability2 { get; set; }
        public AbilityReference AbilityHidden { get; set; }

        public int FormeCount { get; set; }
        public int FormeSprite { get; set; }

        public string Color { get; set; }

        public int BaseExp { get; set; }
        public int BST { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public TypeEffectivenessList TypeEffectiveness { get; set; }
        
        public List<LevelupMoveReference> MoveLevelUp { get; set; }
        public List<MoveReference> MoveTMs { get; set; }
        public List<MoveReference> MoveEgg { get; set; }
        public List<MoveReference> MoveTutors { get; set; }

        public int EscapeRate { get; set; }
        public ItemReference ZItem { get; set; }
        public MoveReference ZBaseMove { get; set; }
        public MoveReference ZMove { get; set; }
        public bool LocalVariant { get; set; }

        public List<EvolutionMethod> Evolutions { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

    }
}
