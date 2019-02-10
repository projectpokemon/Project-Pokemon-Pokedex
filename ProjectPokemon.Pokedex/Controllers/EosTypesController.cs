using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Eos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("eos/types")]
    public class EosTypesController : Controller
    {
        public EosTypesController(DataCollection data)
        {
            _data = data?.EosData ?? throw new ArgumentNullException();
        }

        private EosDataCollection _data;

        // GET: eos/types
        public ActionResult Index()
        {
            return View(_data.Types);
        }

        // GET: eos/types/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(_data.Types[id.Value]);
        }
    }
}