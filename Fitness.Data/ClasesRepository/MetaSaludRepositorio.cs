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
    public class MetaSaludRepositorio : BaseRepositorio<MetaSalud, int?>
    {
        public MetaSaludRepositorio(FTContext db) : base(db){
            lstIncludes.Add("TipoMetaNavigation");
            lstIncludes.Add("TipoPesoNavigation");
        }

        public override async Task<Notificacion<MetaSalud>> Buscar(string filtro, Filtro pf)
        {
            try
            {
                filtro = filtro == null ? "" : filtro;
                var result = new List<MetaSalud>();

                if (_db is null) { return new Notificacion<MetaSalud>(true, Accion.obtenerLista, true); }
                pf.cantidadRegistros = _db.Dieta.Where(x => x.Usuario == pf.usuario).Count();

                result = await _db.MetaSalud
                 .Join(_db.TipoMeta,
                   metaSalud => metaSalud.TipoMeta,
                   tipoMeta => tipoMeta.Id,
                  (metaSalud, tipoMeta) => new { MetaSalud = metaSalud, TipoMeta = tipoMeta })
                 .Where(join => join.MetaSalud.Usuario == pf.usuario && join.TipoMeta.Descripcion.Contains(filtro))
                 .OrderBy(join => join.MetaSalud.FechaObjectivo)
                 .Select(join => join.MetaSalud)
                 .Skip((pf.numeroPagina - 1) * pf.tamanoPagina)
                 .Take(pf.tamanoPagina).ToListAsync<MetaSalud>();

                Notificacion<MetaSalud> notificacion = new Notificacion<MetaSalud>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<MetaSalud> notificacion = new Notificacion<MetaSalud>(true, Accion.obtener, true);
                notificacion.lista = new List<MetaSalud>();
                return notificacion;
            }
        }
    }
}
