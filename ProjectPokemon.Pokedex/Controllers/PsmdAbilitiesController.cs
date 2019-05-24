using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Psmd;
using ProjectPokemon.Pokedex.ViewModels.Psmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("psmd/abilities")]
    public class PsmdAbilitiesController : Controller
    {
        public PsmdAbilitiesController(DataCollection data)
        {
            _data = data?.PsmdData ?? throw new ArgumentNullException();
        }

        private PsmdDataCollection _data;

        // GET: psmd/abilities
        public ActionResult Index()
        {
            return View(_data.Abilities);
        }

        // GET: psmd/abilities/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(new PsmdAbilityDetailsViewModel(_data.Abilities[id.Value], _data));
        }
    }
}
