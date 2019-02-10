using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games
{
    public interface IModelReference
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}
