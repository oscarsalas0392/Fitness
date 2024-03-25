using Fitness.Data;
using Fitness.Data.ClasesRepository;
using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fitness.ViewModels
{
    public class TipoComidaIndexViewModel
    {
        TipoComidaRepositorio? _cR;
        public string Command { get; set; } = "list";
        public TipoComida filtro { get; set; } = new TipoComida();
        public IEnumerable<TipoComida> Items { get; set; } = new List<TipoComida>();
        public Filtro paginacion { get; set; } = new Filtro() { columnaOrdenar = "Descripcion" };

        public async Task HandleRequest(TipoComidaRepositorio cR)
        {
            _cR = cR;

            switch (Command)
            {
                case "list":
                    await list();
                    break;
                case "search":
                case "paging":
                case "order":
                    await search();
                    break;
                default:
                    break;
            }
        }

        async Task list()
        {
            Notificacion<TipoComida> notificacion = await _cR.ObtenerLista(paginacion);
            if(notificacion.lista != null) 
            {
                Items= notificacion.lista;
            }
        }

        async Task search()
        {
            paginacion.columnaBuscar = "Descripcion";
            Notificacion<TipoComida> notificacion = await _cR.Buscar(filtro.Descripcion, paginacion);
            if (notificacion.lista != null)
            {
                Items = notificacion.lista;
            }
        }

    }
}
