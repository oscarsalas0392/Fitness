using Fitness.Data.Clases;
using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class AlimentoConsumidoRepositorio : BaseRepositorio<AlimentoConsumido, int?>
    {
        public AlimentoConsumidoRepositorio(FTContext db) : base(db)
        {
        }

        public override async Task<Notificacion<AlimentoConsumido>> ObtenerLista(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<AlimentoConsumido>();

                if (_db is null) { return new Notificacion<AlimentoConsumido>(true, Accion.obtenerLista, true); }
                pFiltro.cantidadRegistros = _db.AlimentoConsumido.Where(x => x.Dieta == pFiltro.dieta && x.Opcion == (pFiltro.opcionGrupo == null ? 1 : pFiltro.opcionGrupo)).Count();

                result = await _db.AlimentoConsumido
                 .Join(_db.Alimento,
                   aliCon => aliCon.Alimento,
                   ali => ali.Id,
                  (aliCon, ali) => new { AliCon = aliCon, Ali = ali })
                 .Where(join => join.AliCon.Dieta == pFiltro.dieta && join.AliCon.Eliminado == false && join.AliCon.Opcion == (pFiltro.opcionGrupo == null ? 1 : pFiltro.opcionGrupo))
                 .OrderBy(join => join.Ali.Descripcion)
                 .Select(join => join.AliCon)
                 .Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina)
                 .Take(pFiltro.tamanoPagina).ToListAsync<AlimentoConsumido>();

                Notificacion<AlimentoConsumido> notificacion = new Notificacion<AlimentoConsumido>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<AlimentoConsumido> notificacion = new Notificacion<AlimentoConsumido>(true, Accion.obtener, true);
                notificacion.lista = new List<AlimentoConsumido>();
                return notificacion;
            }
        }
        public override async Task<Notificacion<AlimentoConsumido>> Buscar(string filtro, Filtro pf)
        {
            try
            {
                filtro = filtro == null ? "" : filtro;
                var result = new List<AlimentoConsumido>();

                if (_db is null) { return new Notificacion<AlimentoConsumido>(true, Accion.obtenerLista, true); }
                pf.cantidadRegistros = _db.AlimentoConsumido.Where(x => x.Dieta == pf.dieta && x.Opcion == pf.opcionGrupo).Count();

                result = await _db.AlimentoConsumido
                 .Join(_db.Alimento,
                   aliCon => aliCon.Alimento,
                   ali => ali.Id,
                  (aliCon, ali) => new { AliCon = aliCon, Ali = ali })
                 .Where(join => join.AliCon.Dieta == pf.dieta && join.AliCon.Eliminado == false && join.AliCon.Opcion == pf.opcionGrupo && join.Ali.Descripcion.Contains(filtro))
                 .OrderBy(join => join.Ali.Descripcion)
                 .Select(join => join.AliCon)
                 .Skip((pf.numeroPagina - 1) * pf.tamanoPagina)
                 .Take(pf.tamanoPagina).ToListAsync<AlimentoConsumido>();

                Notificacion<AlimentoConsumido> notificacion = new Notificacion<AlimentoConsumido>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<AlimentoConsumido> notificacion = new Notificacion<AlimentoConsumido>(true, Accion.obtener, true);
                notificacion.lista = new List<AlimentoConsumido>();
                return notificacion;
            }
        }
        public async Task<Notificacion<Opcion>> ObtenerOpciones()
        {
            try
            {
                var result = new List<Opcion>();
                if (_db is null) { return new Notificacion<Opcion>(true, Accion.obtener, true); }

                for (int i = 0; i < 10; i++)
                {
                    Opcion opcion = new Opcion();
                    opcion.Id = i + 1;
                    result.Add(opcion);
                }
                Notificacion<Opcion> notificacion = new Notificacion<Opcion>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<Opcion> notificacion = new Notificacion<Opcion>(true, Accion.obtener, true);
                notificacion.lista = new List<Opcion>();
                return notificacion;
            }
        }
        public async Task<Notificacion<Opcion>> ObtenerOpciones(int dieta)
        {
            try
            {
                var result = new List<Opcion>();
                if (_db is null) { return new Notificacion<Opcion>(true, Accion.obtener, true); }

                result = await _db.AlimentoConsumido
                 .Where(x => x.Dieta == dieta && x.Eliminado == false)
                 .GroupBy(x => x.Opcion)
                 .Select(x => new Opcion()
                 {
                     Id = x.Key
                 })
                 .ToListAsync<Opcion>();


                Notificacion<Opcion> notificacion = new Notificacion<Opcion>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<Opcion> notificacion = new Notificacion<Opcion>(true, Accion.obtener, true);
                notificacion.lista = new List<Opcion>();
                return notificacion;
            }
        }
        public async Task<Notificacion<Alimento>> ObtenerAlimentos(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<Alimento>();
                if (_db is null) { return new Notificacion<Alimento>(true, Accion.obtener, true); }

                result = await _db.AlimentoConsumido
                  .Join(_db.Alimento,
                    aliCon => aliCon.Alimento,
                    ali => ali.Id,
                   (aliCon, ali) => new { AliCon = aliCon, Ali = ali })
                  .Where(join => join.AliCon.Dieta == pFiltro.dieta && join.AliCon.Eliminado == false &&  join.AliCon.Opcion == (pFiltro.opcionGrupo == null ? 1 : pFiltro.opcionGrupo))
                  .Select(join => join.Ali).ToListAsync<Alimento>();

                Notificacion<Alimento> notificacion = new Notificacion<Alimento>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<Alimento> notificacion = new Notificacion<Alimento>(true, Accion.obtener, true);
                notificacion.lista = new List<Alimento>();
                return notificacion;
            }
        }

        public Notificacion<int> TotalCalorias(Filtro? pFiltro = null)
        {
            try
            {
                int result = 0;
                if (_db is null) { return new Notificacion<int>(true, Accion.obtener, true); }

                result = _db.AlimentoConsumido
                  .Join(_db.Alimento,
                    aliCon => aliCon.Alimento,
                    ali => ali.Id,
                   (aliCon, ali) => new { AliCon = aliCon, Ali = ali })
                  .Where(join => join.AliCon.Dieta == pFiltro.dieta && join.AliCon.Eliminado == false && join.AliCon.Opcion == (pFiltro.opcionGrupo == null ? 1 : pFiltro.opcionGrupo))
                  .Sum(join => join.Ali.Calorias);

                Notificacion<int> notificacion = new Notificacion<int>(true, Accion.obtenerLista);
                notificacion.objecto = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<int> notificacion = new Notificacion<int>(true, Accion.obtener, true);
                notificacion.objecto = 0;
                return notificacion;
            }
        }

    }
}
