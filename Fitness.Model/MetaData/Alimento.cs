using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Fitness.Model.Models
{
    [ModelMetadataType(typeof(AlimentoMetadata))]
    public partial class Alimento
    {
        public class AlimentoMetadata
        {
            [Required(ErrorMessage = "La descripción es requerida.")]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "Las calorías son requeridas.")]
            public int Calorias { get; set; }

        }
    }
}
