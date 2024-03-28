using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Fitness.Data.ClasesRepository
{
    public class ActividadFisicaRepositorio : BaseRepositorio<ActividadFisica, int?>
    {
        public ActividadFisicaRepositorio(FTContext db) : base(db){}

        public override async Task<Notificacion<ActividadFisica>> Buscar(string filtro, Filtro pf)
        {
            try
            {
                var result = new List<ActividadFisica>();

                if (filtro is null)
                {
                    filtro = "";
                }

                if (_db is null) { return new Notificacion<ActividadFisica>(true, Accion.obtenerLista, true); }
                pf.cantidadRegistros = _db.ActividadFisica
                 .Join(_db.TipoActividadFisica,
                   act => act.TipoActividadFisica,
                   tipo => tipo.Id,
                  (act, tipo) => new { Act = act, Tipo = tipo })
                 .Where(join => join.Tipo.Descripcion.Contains(filtro) && join.Act.Eliminado == false && join.Act.Usuario == pf.usuario)
                  .Count();

                result = _db.ActividadFisica
                 .Join(_db.TipoActividadFisica,
                   act => act.TipoActividadFisica,
                   tipo => tipo.Id,
                  (act, tipo) => new { Act = act, Tipo = tipo })
                 .Where(join => join.Tipo.Descripcion.Contains(filtro) && join.Act.Eliminado == false && join.Act.Usuario == pf.usuario)
                 .Select(join => join.Act)
                 .OrderBy(pf.Ordenando)
                 .Skip((pf.numeroPagina - 1) * pf.tamanoPagina)
                 .Take(pf.tamanoPagina).ToList();
     
                Notificacion<ActividadFisica> notificacion = new Notificacion<ActividadFisica>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<ActividadFisica> notificacion = new Notificacion<ActividadFisica>(true, Accion.obtener, true);
                notificacion.lista = new List<ActividadFisica>();
                return notificacion;
            }
        }

    }
}
