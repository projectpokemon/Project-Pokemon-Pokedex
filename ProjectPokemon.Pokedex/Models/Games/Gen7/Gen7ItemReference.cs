using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7ItemReference : IModelReference
    {
        public Gen7ItemReference(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
