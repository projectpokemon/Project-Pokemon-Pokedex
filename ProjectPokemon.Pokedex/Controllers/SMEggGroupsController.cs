using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("sm/egggroups")]
    public class SMEggGroupsController : Controller
    {
        public SMEggGroupsController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: sm/egggroups
        public ActionResult Index()
        {
            return View(_data.GetEggGroups());
        }

        // GET: sm/egggroups/{id}
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

            ViewBag.ControllerPrefix = "SM";           

            return View(eggGroup);
        }
    }
}