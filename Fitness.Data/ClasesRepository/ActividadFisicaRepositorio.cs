using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Fitness.Data.ClasesRepository
{
    public class ActividadFisicaRepositorio : BaseRepositorio<ActividadFisica, int?>
    {
        public ActividadFisicaRepositorio(FTContext db) : base(db){
            lstIncludes.Add("TipoActividadFisicaNavigation");
            lstIncludes.Add("TipoDistanciaNavigation");
        }
    }
}
