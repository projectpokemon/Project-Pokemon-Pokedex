using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class MoveReference : IModelReference
    {
        public MoveReference()
        {
        }

        public MoveReference(Move move)
        {
            ID = move.ID;
            Name = move.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
