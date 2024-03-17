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

namespace Fitness.Data
{
    public abstract class BaseRepositorio<T, TK> : IRepositorio<T, TK> where T : class
    {
        readonly protected dbContext? _db = null;
        public string _columnaPK { get; set; } = "Id";

        public BaseRepositorio(dbContext db)
        {
            _db = db;
        }

        public virtual async Task<bool> Guardar(T model)
        {
            try
            {
                if (_db is null) { return false; }
                await _db.Set<T>().AddAsync(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;
                return blnResultado;

            }
            catch (Exception)
            {
                return false;
            }

        }


        public virtual async Task<bool> Actualizar(T model)
        {
            try
            {
                if (_db is null) { return false; }
                _db.Set<T>().Update(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;
                return blnResultado;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<T> ObtenerId(TK key)
        {

            try
            {
                if (_db is null) { return null; }
                return await _db.Set<T>().Where($"{_columnaPK} = {key}").FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
 
        }

        public Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

  
        public virtual async Task<IEnumerable<T>> ObtenerLista(Filtro? pFiltro = null)
        {
            var result = new List<T>();

            if (pFiltro == null)
            {
                result = await _db.Set<T>().AsNoTracking().ToListAsync();
            }
            else    
            {
                pFiltro.cantidadRegistros = await _db.Set<T>().CountAsync();
                result = await _db.Set<T>().
                    AsNoTracking().
                    OrderBy(pFiltro.Ordenando).
                    Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina).
                    Take(pFiltro.tamanoPagina).ToListAsync();
            }

            return result;
        }

        public virtual async Task<bool> Eliminar(TK key)
        {
            try
            {
                if (_db is null) { return false; }
                T objecto = await ObtenerId(key);
                PropertyInfo propertyInfo = objecto.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(objecto, Convert.ChangeType(true, propertyInfo.PropertyType), null);
                return await Actualizar(objecto);
            }
            catch (Exception)
            {
                return false;
            }
        
                
        }
    }
}
