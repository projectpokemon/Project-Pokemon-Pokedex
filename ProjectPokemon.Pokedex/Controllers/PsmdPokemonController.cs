using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Psmd;
using ProjectPokemon.Pokedex.ViewModels.Psmd;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("psmd/pokemon")]
    public class PsmdPokemonController : Controller
    {
        public PsmdPokemonController(DataCollection data)
        {
            _data = data?.PsmdData ?? throw new ArgumentNullException();
        }

        private PsmdDataCollection _data;

        // GET: psmd/pokemon
        public ActionResult Index()
        {
            return View(_data.Pokemon);
        }

        // GET: psmd/pokemon/5
        [Route("{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            return View(new PsmdPokemonDetailsViewModel(_data.Pokemon[id.Value], _data));
        }
    }
}