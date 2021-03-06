﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex.Models.Gen7
{
    public class TypeReference : IModelReference
    {
        public TypeReference()
        {
        }

        public TypeReference(PkmType type)
        {
            ID = type.ID;
            Name = type.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
