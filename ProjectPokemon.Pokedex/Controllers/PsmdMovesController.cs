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
    [Route("psmd/moves")]
    public class PsmdMovesController : Controller
    {
        public PsmdMovesController(DataCollection data)
        {
            _data = data?.PsmdData ?? throw new ArgumentNullException();
        }

        private PsmdDataCollection _data;

        // GET: psmd/moves
        public ActionResult Index()
        {
            return View(_data.Moves);
        }

        // GET: psmd/moves/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(new PsmdMoveDetailsViewModel(_data.Moves[id.Value], _data));
        }
    }
}
