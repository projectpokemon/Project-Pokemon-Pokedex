using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class MoveReference : IModelReference
    {
        public MoveReference(int id, string name, SMDataCollection data)
        {
            ID = id;
            Name = name;
            _data = data;
        }

        public MoveReference(Move move, SMDataCollection data)
        {
            ID = move.ID;
            Name = move.Name;
            _move = move;
            _data = data;
        }

        private Move Move
        {
            get
            {
                if (_move != null)
                {
                    return _move;
                }
                else if (_data != null)
                {
                    return _data.Moves.FirstOrDefault(x => x.ID == ID);
                }
                else
                {
                    return null;
                }
            }
        }
        private Move _move;
        private SMDataCollection _data;

        public int ID { get; set; }
        public string Name { get; set; }

        public IEnumerable<Pokemon> GetEggMoveSources(Pokemon pkm)
        {
            return _data.Pokemon.Where(p => (
                                                // Compare egg groups, taking into account the possibility egg group 1 corresponds to egg group 2
                                                p.EggGroup1 == pkm.EggGroup1 ||
                                                p.EggGroup2 == pkm.EggGroup2 ||
                                                p.EggGroup1 == pkm.EggGroup2 ||
                                                p.EggGroup2 == pkm.EggGroup1
                                            ) &&
                                            (
                                                // Ensure the Pokemon in the egg group can learn this move
                                                p.MoveLevelUp.Any(m => m.ID == this.ID)
                                            )
                                        ).OrderBy(p => p.ID);
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
