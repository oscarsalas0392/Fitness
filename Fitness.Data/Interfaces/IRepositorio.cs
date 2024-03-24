
using Fitness.Notificacion;
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
        Task<Notificacion<T>> Guardar(T model);
        Task<Notificacion<T>> Actualizar(T model);
        Task<Notificacion<T>> ObtenerId(TK key);
        Task<Notificacion<T>> ObtenerLista(Filtro? pf = null);
        Task<Notificacion<T>> Eliminar(TK key);
    }
}
