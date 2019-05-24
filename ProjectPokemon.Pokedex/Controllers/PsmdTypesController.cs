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
    [Route("psmd/types")]
    public class PsmdTypesController : Controller
    {
        public PsmdTypesController(DataCollection data)
        {
            _data = data?.PsmdData ?? throw new ArgumentNullException();
        }

        private readonly PsmdDataCollection _data;

        // GET: psmd/types
        public ActionResult Index()
        {
            return View(_data.Types);
        }

        // GET: psmd/types/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(new PsmdTypeDetailsViewModel(_data.Types[id.Value], _data));
        }
    }
}
