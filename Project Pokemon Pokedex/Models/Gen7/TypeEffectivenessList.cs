using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class TypeEffectivenessList : IEnumerable<TypeEffectivenessResult>
    {

        public TypeEffectivenessList(TypeEffectivenessChart chart, List<PkmType> types, int type1Id, int type2Id)
        {
            const int numTypes = 18;
            Results = new List<TypeEffectivenessResult>();

            for (int i = 0; i < numTypes; i++)
            {
                var result = new TypeEffectivenessResult();
                result.Type = new TypeReference(types.FirstOrDefault(x => x.ID == i));
                switch (chart.GetEffectiveness(type1Id, types[i].ID))
                {
                    case TypeEffectiveness.Super:
                        result.Multiplier = 2;
                        break;
                    case TypeEffectiveness.Normal:
                        result.Multiplier = 1;
                        break;
                    case TypeEffectiveness.NotVery:
                        result.Multiplier = 0.5f;
                        break;
                    case TypeEffectiveness.None:
                        result.Multiplier = 0;
                        break;
                }
                if (type1Id != type2Id)
                {
                    switch (chart.GetEffectiveness(type2Id, types[i].ID))
                    {
                        case TypeEffectiveness.Super:
                            result.Multiplier *= 2;
                            break;
                        case TypeEffectiveness.Normal:
                            result.Multiplier *= 1;
                            break;
                        case TypeEffectiveness.NotVery:
                            result.Multiplier *= 0.5f;
                            break;
                        case TypeEffectiveness.None:
                            result.Multiplier *= 0;
                            break;
                    }
                }
            }
            
        }

        public List<TypeEffectivenessResult> Results { get; private set; }

        public IEnumerator<TypeEffectivenessResult> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Results.GetEnumerator();
        }
    }
}
