using Fitness.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Fitness.Notificacion;

namespace Fitness.Data
{
    public abstract class BaseRepositorio<T, TK> : IRepositorio<T, TK> where T : class
    {
        readonly protected FTContext? _db = null;
        public string _columnaPK { get; set; } = "Id";

        public BaseRepositorio(FTContext db)
        {
            _db = db;
        }

        public virtual async Task<Notificacion<T>> Guardar(T model)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.agregar, true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                await _db.Set<T>().AddAsync(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Notificacion<T> notificacion = new Notificacion<T>(blnResultado, Accion.agregar);
                notificacion.objecto = model;
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.agregar, true);
            }
        }
        public virtual async Task<Notificacion<T>> Actualizar(T model)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.actualizar,true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                _db.Set<T>().Update(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Notificacion<T> notificacion = new Notificacion<T>(blnResultado,Accion.actualizar);
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.actualizar, true);
            }
        }
        public virtual async Task<Notificacion<T>> ObtenerId(TK? key)
        {

            try
            {
                if (_db is null || key is null) { return new Notificacion<T>(false, Accion.obtener);}
                T? objecto = await _db.Set<T>().Where($"{_columnaPK} = {key}").FirstOrDefaultAsync();
                Notificacion<T> notificacion = new Notificacion<T>(objecto is not null, Accion.obtener);
                notificacion.objecto = objecto;
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.obtener, true);
            }
 
        }
        public Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            
        }

        public virtual async Task<Notificacion<T>> ObtenerLista(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<T>();
                if (_db is null) { return new Notificacion<T>(true, Accion.obtener, true); }

                if (pFiltro == null)
                {
                    result = await _db.Set<T>().Where($"Eliminado = false").AsNoTracking().ToListAsync();
                }
                else
                {
                    pFiltro.cantidadRegistros = await _db.Set<T>()
                        .Where($"Eliminado = false {(pFiltro.usuario != null ? $" && Usuario = {pFiltro.usuario}" : "")}")
                        .CountAsync();

                    result = await _db.Set<T>().
                        AsNoTracking().
                        Where($"Eliminado = false {(pFiltro.usuario != null ? $" && Usuario = {pFiltro.usuario}" : "")}").
                        OrderBy(pFiltro.Ordenando).
                        Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina).
                        Take(pFiltro.tamanoPagina).ToListAsync();
                }

                Notificacion<T> notificacion = new Notificacion<T>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {  
                Notificacion<T> notificacion = new Notificacion<T>(true, Accion.obtener, true);
                notificacion.lista = new List<T>();
                return notificacion;
            }
        }

        public virtual async Task<Notificacion<T>> Eliminar(TK key)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.obtener, true); }
                Notificacion<T> notificacion = await ObtenerId(key);
                if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
                {
                    return notificacion;
                }

                T? objecto = notificacion.objecto;
                PropertyInfo propertyInfo = objecto.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(objecto, true);
                Notificacion<T> notificacionActualizar = await Actualizar(objecto);
                notificacionActualizar._accion = Accion.eliminar;
                return notificacionActualizar;             
            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.eliminar, true);
            } 
                
        }

        public virtual async Task<Notificacion<T>> Buscar(string filtro,Filtro pf)
        {
            try
            {
                var result = new List<T>();

                if (_db is null) { return new Notificacion<T>(true, Accion.obtenerLista, true); }
                pf.cantidadRegistros = await _db.Set<T>().CountAsync();

                result = await _db.Set<T>().AsNoTracking().
                    Where($"{pf.columnaBuscar}.Contains(\"{filtro}\") && Eliminado = false").
                    OrderBy(pf.Ordenando).
                    Skip((pf.numeroPagina - 1) * pf.tamanoPagina).
                    Take(pf.tamanoPagina).ToListAsync();

                Notificacion<T> notificacion = new Notificacion<T>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;     
            }
            catch(Exception) 
            {
                Notificacion<T> notificacion = new Notificacion<T>(true, Accion.obtener, true);
                notificacion.lista = new List<T>();
                return notificacion;
            }
        }
    }
}
