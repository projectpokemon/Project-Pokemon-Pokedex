using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("usum/moves")]
    public class USUMMovesController : Controller
    {
        public USUMMovesController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: usum/moves
        public ActionResult Index()
        {
            return View(_data.Moves);
        }

        // GET: usum/moves/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.ControllerPrefix = "USUM";

            return View(_data.Moves[id.Value]);
        }
    }
}
