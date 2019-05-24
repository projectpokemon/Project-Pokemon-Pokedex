using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("sm/pokemon")]
    public class SMPokemonController : Controller
    {
        public SMPokemonController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: sm/pokemon
        public ActionResult Index()
        {
            return View(_data.Pokemon);
        }

        // GET: sm/pokemon/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.ControllerPrefix = "SM";

            return View(_data.Pokemon[id.Value]);
        }
    }
}
