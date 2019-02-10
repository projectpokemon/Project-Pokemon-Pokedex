using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7TypeEffectivenessList : IEnumerable<Gen7TypeEffectivenessResult>
    {
        public Gen7TypeEffectivenessList(Gen7TypeEffectivenessChart chart, List<Gen7PkmType> types, int type1Id, int type2Id)
        {
            const int numTypes = 18;
            Results = new List<Gen7TypeEffectivenessResult>();

            for (int i = 0; i < numTypes; i++)
            {
                var result = new Gen7TypeEffectivenessResult();
                result.Type = new Gen7TypeReference(types.FirstOrDefault(x => x.ID == i));
                switch (chart.GetEffectiveness(type1Id, types[i].ID))
                {
                    case Gen7TypeEffectiveness.Super:
                        result.Multiplier = 2;
                        break;
                    case Gen7TypeEffectiveness.Normal:
                        result.Multiplier = 1;
                        break;
                    case Gen7TypeEffectiveness.NotVery:
                        result.Multiplier = 0.5f;
                        break;
                    case Gen7TypeEffectiveness.None:
                        result.Multiplier = 0;
                        break;
                }
                if (type1Id != type2Id)
                {
                    switch (chart.GetEffectiveness(type2Id, types[i].ID))
                    {
                        case Gen7TypeEffectiveness.Super:
                            result.Multiplier *= 2;
                            break;
                        case Gen7TypeEffectiveness.Normal:
                            result.Multiplier *= 1;
                            break;
                        case Gen7TypeEffectiveness.NotVery:
                            result.Multiplier *= 0.5f;
                            break;
                        case Gen7TypeEffectiveness.None:
                            result.Multiplier *= 0;
                            break;
                    }
                }
                Results.Add(result);
            }

        }

        public List<Gen7TypeEffectivenessResult> Results { get; private set; }

        public IEnumerator<Gen7TypeEffectivenessResult> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Results.GetEnumerator();
        }
    }
}
