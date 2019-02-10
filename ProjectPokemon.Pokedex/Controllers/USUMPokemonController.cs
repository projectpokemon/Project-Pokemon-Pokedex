using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("usum/pokemon")]
    public class USUMPokemonController : Controller
    {
        public USUMPokemonController(DataCollection data)
        {
            _data = data?.SMData ?? throw new ArgumentNullException();
        }

        private Gen7DataCollection _data;

        // GET: usum/pokemon
        public ActionResult Index()
        {
            return View(_data.Pokemon);
        }

        // GET: usum/pokemon/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(_data.Pokemon[id.Value]);
        }
    }
}
