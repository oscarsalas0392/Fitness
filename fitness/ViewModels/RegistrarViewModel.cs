using Fitness.Model.Models;

namespace Fitness.ViewModels
{
    public class RegistrarViewModel
    {
        public List<Genero> lstGenero { get; set; } =  new List<Genero>();
        
        public Usuario? usuario { get; set; } 

    }
}
