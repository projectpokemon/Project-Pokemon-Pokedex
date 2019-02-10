using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Games.Gen7
{
    public class Gen7MoveReference : IModelReference
    {
        public Gen7MoveReference(int id, string name, Gen7DataCollection data)
        {
            ID = id;
            Name = name;
            _data = data;
        }

        public Gen7MoveReference(Gen7Move move, Gen7DataCollection data)
        {
            ID = move.ID;
            Name = move.Name;
            _move = move;
            _data = data;
        }

        private Gen7Move Move
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
        private Gen7Move _move;
        private Gen7DataCollection _data;

        public int ID { get; set; }
        public string Name { get; set; }

        public IEnumerable<Gen7Pokemon> GetEggMoveSources(Gen7Pokemon pkm)
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
                                                p.MoveLevelUp.Any(m => m.ID == this.ID) ||
                                                p.GetEggMoves().Any(m => m.ID == this.ID)
                                            ) &&
                                            (
                                                // Filter out mega evolutions because they can't breed
                                                !p.GetIsMega()
                                            ) &&
                                            (
                                                // Filter out members of the current evolution chain
                                                !p.GetEvolutionChain().Any(c => c.TargetPokemon.ID == pkm.ID)
                                            )
                                        ).OrderBy(p => p.ID);
        }

        public bool RequiresChainBreeding(Gen7Pokemon pkm)
        {
            return !_data.Pokemon.Where(p => (
                                                // Compare egg groups, taking into account the possibility egg group 1 corresponds to egg group 2
                                                p.EggGroup1 == pkm.EggGroup1 ||
                                                p.EggGroup2 == pkm.EggGroup2 ||
                                                p.EggGroup1 == pkm.EggGroup2 ||
                                                p.EggGroup2 == pkm.EggGroup1
                                            ) &&
                                            (
                                                // Ensure the Pokemon in the egg group can learn this move
                                                p.MoveLevelUp.Any(m => m.ID == this.ID)
                                            ) &&
                                            (
                                                // Filter out members of the current evolution chain
                                                !p.GetEvolutionChain().Any(c => c.TargetPokemon.ID == pkm.ID)
                                            )
                                        ).Any();
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
