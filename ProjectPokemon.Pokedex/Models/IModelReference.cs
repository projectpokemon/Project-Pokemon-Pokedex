using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models
{
    public interface IModelReference
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}
