using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class Pokemon
    {
        public Pokemon(SMDataCollection data)
        {
            Data = data;
            AltForms = new List<PokemonReference>();
            MegaEvolutions = new List<PokemonReference>();
        }

        public SMDataCollection Data { get; private set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; } // The _____ Pokémon
        public string PokedexTextSun { get; set; }
        public string PokedexTextMoon { get; set; }

        public int BaseHP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseSpeed { get; set; }
        public int BaseSpAttack { get; set; }
        public int BaseSpDefense { get; set; }
        public int HPEVYield { get; set; }
        public int AttackEVYield { get; set; }
        public int DefenseEVYield { get; set; }
        public int SpeedEVYield { get; set; }
        public int SpAttackEVYield { get; set; }
        public int SpDefenseEVYield { get; set; }

        public TypeReference Type1 { get; set; }
        public TypeReference Type2 { get; set; }

        public int CatchRate { get; set; }
        public int EvoStage { get; set; }

        /// <summary>
        /// Possible hold item with a 50% chance of being held
        /// </summary>
        public ItemReference HeldItem1 { get; set; }

        /// <summary>
        /// Possible hold item with a 5% chance of being held
        /// </summary>
        public ItemReference HeldItem2 { get; set; }

        /// <summary>
        /// Possible hold item with a 1% chance of being held
        /// </summary>
        public ItemReference HeldItem3 { get; set; }

        public int Gender { get; set; }
        public int HatchCycles { get; set; }
        public int BaseFriendship { get; set; }

        public string ExpGrowth { get; set; }

        public string EggGroup1 { get; set; }
        public string EggGroup2 { get; set; }
        
        public AbilityReference Ability1 { get; set; }
        public AbilityReference Ability2 { get; set; }
        public AbilityReference AbilityHidden { get; set; }

        public int FormeCount { get; set; }
        public int FormeSprite { get; set; }

        public string Color { get; set; }

        public int BaseExp { get; set; }
        public int BST { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public TypeEffectivenessList TypeEffectiveness { get; set; }
        
        public List<LevelupMoveReference> MoveLevelUp { get; set; }
        public List<MoveReference> MoveTMs { get; set; }
        public List<MoveReference> MoveEgg { get; set; }
        public List<MoveReference> MoveTutors { get; set; }

        public int EscapeRate { get; set; }
        public ItemReference ZItem { get; set; }
        public MoveReference ZBaseMove { get; set; }
        public MoveReference ZMove { get; set; }
        public bool LocalVariant { get; set; }

        public List<EvolutionMethod> Evolutions { get; set; }
        public List<PokemonReference> AltForms { get; set; }
        public List<PokemonReference> MegaEvolutions { get; set; }

        public bool EvolvesToAltForm
        {
            get
            {
                return Evolutions.Any(x => x.Form != -1);
            }
        }

        public string PokespriteHtml
        {
            get
            {           
                if (GetIsAltForm())
                {
                    string form = GetFormName();
                    var originalId = GetOriginalFormId();
                    if (form != "")
                    {
                        var originalPkm = Data.Pokemon[originalId.Value];
                        return $"<span class=\"pkspr pkmn-{originalPkm.Name.ToLower()} form-{form.ToLower().Replace(' ', '-')}\"><span style=\"display: none;\">&nbsp;</span></span>";
                    }
                    else
                    {
                        return $"<span class=\"pkspr pkmn-{GetPokespriteSpeciesName()}\"><span style=\"display: none;\">&nbsp;</span></span>";
                    }
                }   
                else
                {
                    return $"<span class=\"pkspr pkmn-{GetPokespriteSpeciesName()}\"><span style=\"display: none;\">&nbsp;</span></span>";
                }                
            }            
        }

        private string GetPokespriteSpeciesName()
        {
            return Name.ToLower()
                .Replace(' ', '-')
                .Replace(":", "")
                .Replace("'", "")
                .Replace("’", "")
                .Replace(".", "")
                .Replace("♂", "-m")
                .Replace("♀", "-f");
        }

        public string GetFormName()
        {
            if (GetIsAltForm())
            {
                var originalId = GetOriginalFormId();
                if (originalId.HasValue)
                {
                    var originalPkm = Data.Pokemon[originalId.Value];
                    var formIndex = originalPkm.AltForms.IndexOf(originalPkm.AltForms.First(a => a.ID == this.ID)) + 1; // Add 1 to take into account the original form
                    return originalPkm.GetPkhexAltFormStrings()[formIndex];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string[] GetPkhexAltFormStrings()
        {
            string[] gendersymbols = { "♂", "♀", "-" };
            var typeNames = PKHeX.Core.Util.GetTypesList("en");
            var formNames = PKHeX.Core.Util.GetFormsList("en");
            return PKHeX.Core.PKX.GetFormList(ID, typeNames, formNames, gendersymbols, 7);
        }

        public Pokemon GetPreviousEvolution()
        {
            return Data.Pokemon.Where(p => p.Evolutions.Any(e => e.TargetPokemon.ID == ID)).FirstOrDefault();
        }

        public List<MoveReference> GetEggMoves()
        {
            if (MoveEgg == null || MoveEgg.Count == 0)
            {
                return GetPreviousEvolution()?.GetEggMoves() ?? MoveEgg;
            }
            else
            {
                return MoveEgg;
            }
        }

        private void AddFutureEvolutions(Stack<EvolutionMethod> methods, Pokemon pkm)
        {
            foreach (var item in pkm.Evolutions.Select(x => x).Reverse()) // the .Select is used for the LINQ reverse
            {
                // Find future evolutions for the next Pokemon
                AddFutureEvolutions(methods, Data.Pokemon[item.TargetPokemon.ID]);

                if (!methods.Contains(item))
                {
                    methods.Push(item);
                }                    
            }            
        }

        public IEnumerable<PokemonReference> GetNonMegaAltForms()
        {
            var theseForms = AltForms.Where(a => !MegaEvolutions.Any(e => e.ID == a.ID));
            var others = Data.Pokemon.Where(p => p.AltForms.Any(a => a.ID == this.ID)).Select(p => new PokemonReference(p));
            return theseForms.Concat(others);
        }

        public bool GetIsAltForm()
        {
            return Data.Pokemon.Any(p => p.AltForms.Any(a => a.ID == this.ID));
        }

        public int? GetOriginalFormId()
        {
            return Data.Pokemon.FirstOrDefault(p => p.AltForms.Any(a => a.ID == this.ID))?.ID ?? null;
        }

        public bool GetIsMega()
        {
            return Data.Pokemon.Any(p => p.MegaEvolutions.Any(m => m.ID == this.ID));
        }

        public List<EvolutionMethod> GetEvolutionChain()
        {
            var methods = new Stack<EvolutionMethod>();

            AddFutureEvolutions(methods, this);            

            var previousEvolution = GetPreviousEvolution();
            if (previousEvolution != null)
            {
                foreach (var item in previousEvolution.GetEvolutionChain().Select(x => x).Reverse()) // the .Select is used for the LINQ reverse
                {
                    if (!methods.Contains(item))
                    {
                        methods.Push(item);
                    }                    
                }
            }
            else
            {
                // Add current Pokemon as a dummy entry
                // This shows the base form when viewing evolutions
                methods.Push(new EvolutionMethod { Form = -1, Method = "", TargetPokemon = new PokemonReference(this) });
            }

            return methods.ToList();
        }

        public string GetCrossReferenceHtml()
        {
            var html = new StringBuilder();

            if (Data.IsUltra)
            {
                html.AppendLine("<b>Ultra Sun and Ultra Moon</b>");
            }
            else
            {
                html.AppendLine("<a href=\"{page=\"ultrasm/usum-pkm-" + ID.ToString() + "\"}\">Ultra Sun and Ultra Moon</a>");
            }

            html.AppendLine(" | ");

            if (Data.IsUltra)
            {
                html.AppendLine("<a href=\"{page=\"sm/sm-pkm-" + ID.ToString() + "\"}\">Sun and Moon</a>");
            }
            else
            {
                html.AppendLine("<b>Sun and Moon</b>");
            }

            var psmd = Data.ParentCollection.PsmdData.Pokemon.Where(p => p.DexNumber == ID).FirstOrDefault();
            if (psmd != null)
            {
                html.AppendLine(" | ");
                html.AppendLine("<a href=\"{page=\"psmd/psmd-pkm-" + ID.ToString() + "\"}\">Super Mystery Dungeon</a>");
            }

            var eos = Data.ParentCollection.EosData.Pokemon.Where(p => p.DexNumber == ID).FirstOrDefault();
            if (eos != null)
            {
                html.AppendLine(" | ");
                html.AppendLine("<a href=\"{page=\"eos/eos-pkm-" + ID.ToString() + "\"}\">Explorers of Sky</a>");
            }

            return html.ToString();
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

    }
}
