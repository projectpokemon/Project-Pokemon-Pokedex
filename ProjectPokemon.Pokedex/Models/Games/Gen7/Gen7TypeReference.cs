using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7TypeReference : IModelReference
    {
        public Gen7TypeReference()
        {
        }

        public Gen7TypeReference(Gen7PkmType type)
        {
            ID = type.ID;
            Name = type.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
