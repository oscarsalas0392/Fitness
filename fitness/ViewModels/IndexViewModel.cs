using Fitness.Data;
using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fitness.ViewModels
{
    public class IndexViewModel<T,TR,TK> where T: class where TR : BaseRepositorio<T,TK>
    {
        TR? _cR ;
        public string Command { get; set; } = "list";
        public TipoComida filtro { get; set; } = new TipoComida();
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public Filtro paginacion { get; set; } = new Filtro() { columnaOrdenar = "Descripcion", columnaBuscar= "Descripcion" };


        public async Task HandleRequest(TR cR, string columnaOrdenar = "Descripcion", string columnaBuscar = "Descripcion", int? usuario = null, bool alimento =false)
        {
            paginacion.columnaBuscar = columnaBuscar;
            paginacion.columnaOrdenar = columnaOrdenar;

            if (usuario is not null)
            {
                if (!alimento)
                {
                    paginacion.usuario = usuario;
                }
                else
                {
                    paginacion.dieta = usuario;
                }
            }
            _cR = cR;
            await EjecutarComando(Command);
        }

        private async Task EjecutarComando (string comando)
        {
            switch (comando)
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
         
            Notificacion<T> notificacion = await _cR.ObtenerLista(paginacion);
            if(notificacion.lista != null) 
            {
                Items= notificacion.lista;
            }
        }

        async Task search()
        {
            Notificacion<T> notificacion = await _cR.Buscar(filtro.Descripcion, paginacion);
            if (notificacion.lista != null)
            {
                Items = notificacion.lista;
            }
        }

    }
}
