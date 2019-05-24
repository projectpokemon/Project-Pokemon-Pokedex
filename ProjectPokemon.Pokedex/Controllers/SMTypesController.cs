using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("sm/types")]
    public class SMTypesController : Controller
    {
        public SMTypesController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: sm/types
        public ActionResult Index()
        {
            return View(_data.Types);
        }

        // GET: sm/types/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.ControllerPrefix = "SM";

            return View(_data.Types[id.Value]);
        }
    }
}
