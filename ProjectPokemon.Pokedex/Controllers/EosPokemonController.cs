﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Eos;

namespace ProjectPokemon.Pokedex.Controllers
{
    [Route("eos/pokemon")]
    public class EosPokemonController : Controller
    {
        public EosPokemonController(DataCollection data)
        {
            _data = data?.EosData ?? throw new ArgumentNullException();
        }

        private EosDataCollection _data;

        // GET: eos/pokemon
        public ActionResult Index()
        {
            return View(_data.Pokemon.Select(p => new EosPokemonReference(_data, p)));
        }

        // GET: eos/pokemon/5
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