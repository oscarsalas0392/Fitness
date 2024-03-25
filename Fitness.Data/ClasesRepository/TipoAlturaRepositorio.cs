using Fitness.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class TipoAlturaRepositorio : BaseRepositorio<TipoAltura, int?>
    {
        public TipoAlturaRepositorio(FTContext db) : base(db){}
    }
}
