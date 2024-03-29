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
            lstIncludes.Add("AlimentoNavigation");
        }
        public Notificacion<Opcion> ObtenerOpciones()
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
