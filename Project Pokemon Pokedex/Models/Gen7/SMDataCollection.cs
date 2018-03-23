using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class SMDataCollection
    {
        public List<Pokemon> Pokemon { get; set; }
        public List<Move> Moves { get; set; }
        public List<PkmType> Types { get; set; }
        public string[][] AltFormStrings { get; set; }
        public bool IsUltra { get; set; }

        public DataCollection ParentCollection { get; set; }

        public TypeEffectivenessChart TypeEffectiveness { get; set; }

        public List<EggGroup> GetEggGroups()
        {
            var groupNames = Pokemon.Select(x => x.EggGroup1).Concat(Pokemon.Select(x => x.EggGroup2)).Distinct().OrderBy(x => x);
            var groups = new List<EggGroup>();
            foreach (var item in groupNames)
            {
                var group = new EggGroup();
                group.Name = item;
                group.SingleEggGroupPokemon = Pokemon.Where(x => x.EggGroup1 == item || x.EggGroup2 == item).Where(x => x.EggGroup1 == x.EggGroup2).Distinct().ToList();
                group.MultiEggGroupPokemon = Pokemon.Where(x => x.EggGroup1 == item || x.EggGroup2 == item).Where(x => x.EggGroup1 != x.EggGroup2).Distinct().ToList();
            }
            return groups;
        }
    }
}
