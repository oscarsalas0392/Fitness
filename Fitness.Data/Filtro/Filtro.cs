﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data
{
    public class Filtro
    {
        public int numeroPagina { get; set; }
        public int tamanoPagina { get; set; }
        public int elementosPagina => (int)Math.Ceiling((decimal)cantidadRegistros / tamanoPagina);
        public string columnaOrdenar { get; set; } = "";
        public string columnaBuscar { get; set; } = "";
        public string tipoOrdernar { get; set; } = "";
        public string Ordenando
        {
            get
            {
                if (string.IsNullOrEmpty(columnaOrdenar))
                {
                    throw new InvalidOperationException("The PageFilter needs a default sort.");
                }

                return string.IsNullOrEmpty(columnaOrdenar) ? "" : $"{columnaOrdenar} {tipoOrdernar}";
            }
        }
        public int cantidadRegistros { get; set; }
        public int? usuario { get; set; } = null;
        public int? dieta { get; set; } = null;
        public int? opcionGrupo { get; set; } = null;

        public List<string> lstIncludes { get; set; } = new List<string>();
        public Filtro()
        {
            numeroPagina = 1;
            tamanoPagina = 10;
            columnaOrdenar = "";
            tipoOrdernar = "ASC";
        }
    }
}
