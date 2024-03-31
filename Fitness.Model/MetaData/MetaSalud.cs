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
        [ModelMetadataType(typeof(MetaSaludMetadata))]
        public partial class MetaSalud
        {
            public class MetaSaludMetadata
            {
                [Required(ErrorMessage = "El tipo de meta es requerido.")]
                public int TipoMeta { get; set; }

                [Required(ErrorMessage = "El peso objectivo es requerido.")]
                public decimal PesoObjectivo { get; set; }

                [Required(ErrorMessage = "El tipo de peso es requerido.")]
                public int TipoPeso { get; set; }

                [Required(ErrorMessage = "La fecha objectivo es requerida")]
                public DateTime FechaObjectivo { get; set; }

                [Required(ErrorMessage = "El nivel de actividad es requerida.")]
                public string NivelActividad { get; set; }

            }
        }
    
}
