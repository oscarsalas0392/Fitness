using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Model.Models
{
    [ModelMetadataType(typeof(ActividadFisicaMetadata))]
    public partial class ActividadFisica
    {
        public class ActividadFisicaMetadata
        {
            [Required(ErrorMessage = "El tipo de actividad es requerida.")]
            public int TipoActividadFisica { get; set; }

            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }

            [Required(ErrorMessage = "La duración es requerida.")]
            public int Duracion { get; set; }

            [Required(ErrorMessage = "Las calorías son requeridas.")]
            public int Calorias { get; set; }
        }
    }
}
