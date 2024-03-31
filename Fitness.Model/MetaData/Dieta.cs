using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Model.Models
{
    public partial class Dieta
    {
        [ModelMetadataType(typeof(DietaMetadata))]
        public class DietaMetadata
        {
            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }

            [Required(ErrorMessage = "El tipo de comida es requerido.")]
            public int TipoComida { get; set; }

        }
    }
}
