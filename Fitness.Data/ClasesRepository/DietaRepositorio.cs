using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class DietaRepositorio : BaseRepositorio<Dieta, int?>
    {
        public DietaRepositorio(FTContext db) : base(db){}

        public override async Task<Notificacion<Dieta>> ObtenerLista(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<Dieta>();

                if (_db is null) { return new Notificacion<Dieta>(true, Accion.obtenerLista, true); }
                pFiltro.cantidadRegistros = _db.Dieta.Where(x => x.Usuario == pFiltro.usuario).Count();

                result = await _db.Dieta
                 .Join(_db.TipoComida,
                   dieta => dieta.TipoComida,
                   tipoComida => tipoComida.Id,
                  (dieta, tipoComida) => new { Dieta = dieta, TipoComida = tipoComida })
                 .Where(join => join.Dieta.Usuario == pFiltro.usuario)
                 .OrderBy(join => join.TipoComida.Descripcion)
                 .Select(join => join.Dieta)
                 .Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina)
                 .Take(pFiltro.tamanoPagina).ToListAsync<Dieta>();

                Notificacion<Dieta> notificacion = new Notificacion<Dieta>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<Dieta> notificacion = new Notificacion<Dieta>(true, Accion.obtener, true);
                notificacion.lista = new List<Dieta>();
                return notificacion;
            }
        }
        public override async Task<Notificacion<Dieta>> Buscar(string filtro, Filtro pf)
        {
            try
            {
                filtro = filtro == null ? "" : filtro;
                var result = new List<Dieta>();

                if (_db is null) { return new Notificacion<Dieta>(true, Accion.obtenerLista, true); }
                pf.cantidadRegistros = _db.Dieta.Where(x => x.Usuario == pf.usuario).Count();

                result = await _db.Dieta
                 .Join(_db.TipoComida,
                   dieta => dieta.TipoComida,
                   tipoComida => tipoComida.Id,
                  (dieta, tipoComida) => new { Dieta = dieta, TipoComida = tipoComida })
                 .Where(join => join.Dieta.Usuario == pf.usuario && join.TipoComida.Descripcion.Contains(filtro))
                 .OrderBy(join => join.TipoComida.Descripcion)
                 .Select(join => join.Dieta)
                 .Skip((pf.numeroPagina - 1) * pf.tamanoPagina)
                 .Take(pf.tamanoPagina).ToListAsync<Dieta>();

                Notificacion<Dieta> notificacion = new Notificacion<Dieta>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<Dieta> notificacion = new Notificacion<Dieta>(true, Accion.obtener, true);
                notificacion.lista = new List<Dieta>();
                return notificacion;
            }
        }
    }
}
