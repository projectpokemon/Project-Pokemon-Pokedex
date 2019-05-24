using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("usum/egggroups")]
    public class USUMEggGroupsController : Controller
    {
        public USUMEggGroupsController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private readonly Gen7DataCollection _data;

        // GET: usum/egggroups
        public ActionResult Index()
        {
            return View(_data.GetEggGroups());
        }

        // GET: usum/egggroups/{id}
        [Route("{id}")]
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var eggGroup = _data.GetEggGroups().FirstOrDefault(eg => StringComparer.OrdinalIgnoreCase.Compare(eg.Name, id) == 0);
            if (eggGroup == null)
            {
                return NotFound();
            }

            ViewBag.ControllerPrefix = "USUM";

            return View(eggGroup);
        }
    }
}
