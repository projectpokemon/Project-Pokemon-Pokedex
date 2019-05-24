using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("sm/moves")]
    public class SMMovesController : Controller
    {
        public SMMovesController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: sm/moves
        public ActionResult Index()
        {
            return View(_data.Moves);
        }

        // GET: sm/moves/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.ControllerPrefix = "SM";

            return View(_data.Moves[id.Value]);
        }
    }
}