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
    public class BitacoraPesoRepositorio : BaseRepositorio<BitacoraPeso, int?>
    {
        public BitacoraPesoRepositorio(FTContext db) : base(db)
        {
        }

        public  async Task<Notificacion<BitacoraPeso>> ObtenerListaPesos(int usuario)
        {
            try
            {
                var result = new List<BitacoraPeso>();
                if (_db is null) { return new Notificacion<BitacoraPeso>(true, Accion.obtener, true); }
                result = await _db.BitacoraPeso.Where(x => x.Usuario == usuario).ToListAsync<BitacoraPeso>();
                Notificacion<BitacoraPeso> notificacion = new Notificacion<BitacoraPeso>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<BitacoraPeso> notificacion = new Notificacion<BitacoraPeso>(true, Accion.obtener, true);
                notificacion.lista = new List<BitacoraPeso>();
                return notificacion;
            }
        }


    }
}
