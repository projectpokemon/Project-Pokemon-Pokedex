using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7TypeEffectivenessChart
    {
        public Gen7TypeEffectivenessChart(byte[] rawData)
        {
            const int numTypes = 18;
            EffectivenessChart = new Gen7TypeEffectiveness[numTypes, numTypes];
            for (int i = 0; i < rawData.Length; i++)
            {
                int attackType = i % numTypes;
                int defenseType = i / numTypes;

                switch (rawData[i])
                {
                    case 8:
                        EffectivenessChart[attackType, defenseType] = Gen7TypeEffectiveness.Super;
                        break;
                    case 4:
                        EffectivenessChart[attackType, defenseType] = Gen7TypeEffectiveness.Normal;
                        break;
                    case 2:
                        EffectivenessChart[attackType, defenseType] = Gen7TypeEffectiveness.NotVery;
                        break;
                    case 0:
                        EffectivenessChart[attackType, defenseType] = Gen7TypeEffectiveness.None;
                        break;
                    default:
                        throw new Exception("Unknown effectiveness: " + rawData[i].ToString());
                }
            }
        }

        private Gen7TypeEffectiveness[,] EffectivenessChart;

        public Gen7TypeEffectiveness GetEffectiveness(int attackTypeId, int defenseTypeId)
        {
            return EffectivenessChart[attackTypeId, defenseTypeId];
        }
    }
}
