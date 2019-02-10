using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Eos;

namespace ProjectPokemon.Pokedex.Controllers
{
    public class EosPokemonController : Controller
    {
        public EosPokemonController(DataCollection data)
        {
            _data = data?.EosData ?? throw new ArgumentNullException();
        }

        private EosDataCollection _data;

        // GET: EosPokemon
        public ActionResult Index()
        {
            return View(_data.Pokemon.Select(p => new EosPokemonReference(_data, p)));
        }

        //// GET: EosPokemon/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}
    }
}