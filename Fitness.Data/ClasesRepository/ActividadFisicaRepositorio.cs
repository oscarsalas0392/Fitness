using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class ActividadFisicaRepositorio : BaseRepositorio<ActividadFisicaRepositorio, int>
    {
        public ActividadFisicaRepositorio(dbContext db) : base(db){}
    }
}
