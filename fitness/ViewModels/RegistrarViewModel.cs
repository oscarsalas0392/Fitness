using Fitness.Model.Models;

namespace Fitness.ViewModels
{
    public class RegistrarViewModel
    {
        public List<Genero> lstGenero { get; set; } =  new List<Genero>();
        public List<TipoPeso> lstPeso { get; set; } = new List<TipoPeso>();
        public List<TipoAltura> lstAltura { get; set; } = new List<TipoAltura>();
        public Usuario? usuario { get; set; } 

    }
}
