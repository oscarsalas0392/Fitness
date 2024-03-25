using Fitness.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class TipoPesoRepositorio : BaseRepositorio<TipoPeso, int?>
    {
        public TipoPesoRepositorio(FTContext db) : base(db){}
    }
}
