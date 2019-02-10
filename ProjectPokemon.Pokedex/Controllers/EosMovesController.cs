using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Eos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("eos/moves")]
    public class EosMovesController : Controller
    {
        public EosMovesController(DataCollection data)
        {
            _data = data?.EosData ?? throw new ArgumentNullException();
        }

        private EosDataCollection _data;

        // GET: eos/moves
        public ActionResult Index()
        {
            return View(_data.Moves.Select(m => new EosMoveReference(_data, m)));
        }

        // GET: eos/moves/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(_data.Moves[id.Value]);
        }
    }
}
