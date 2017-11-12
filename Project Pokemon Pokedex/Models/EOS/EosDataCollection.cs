using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.EOS
{
    public class EosDataCollection
    {
        public List<Pokemon> Pokemon { get; set; }
        public List<Move> Moves { get; set; }
        public List<PkmType> Types { get; set; }

        public DataCollection ParentCollection { get; set; }
    }
}
