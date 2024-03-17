using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.Interfaces
{
    public interface IRepositorio<T, TK> : IDisposable
    {
        Task<bool> Guardar(T model);
        Task<bool> Actualizar(T model);
        Task<T> ObtenerId(TK key);
        Task<IEnumerable<T>> ObtenerLista(Filtro? pf = null);
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
        Task<bool> Eliminar(TK key);
    }
}
