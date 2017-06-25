using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class TypeEffectivenessChart
    {
        public TypeEffectivenessChart(byte[] rawData)
        {
            const int numTypes = 18;
            EffectivenessChart = new TypeEffectiveness[numTypes, numTypes];
            for (int i = 0;i<rawData.Length;i++)
            {
                int attackType = i % numTypes;
                int defenseType = i / numTypes;

                switch (rawData[i])
                {
                    case 8:
                        EffectivenessChart[attackType, defenseType] = TypeEffectiveness.Super;
                        break;
                    case 4:
                        EffectivenessChart[attackType, defenseType] = TypeEffectiveness.Normal;
                        break;
                    case 2:
                        EffectivenessChart[attackType, defenseType] = TypeEffectiveness.NotVery;
                        break;
                    case 0:
                        EffectivenessChart[attackType, defenseType] = TypeEffectiveness.None;
                        break;
                    default:
                        throw new Exception("Unknown effectiveness: " + rawData[i].ToString());
                }
            }
        }

        private TypeEffectiveness[,] EffectivenessChart;

        public TypeEffectiveness GetEffectiveness(int attackTypeId, int defenseTypeId)
        {
            return EffectivenessChart[attackTypeId, defenseTypeId];
        }
    }
}
