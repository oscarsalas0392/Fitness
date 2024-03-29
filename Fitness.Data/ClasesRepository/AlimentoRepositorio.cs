using Fitness.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class AlimentoRepositorio : BaseRepositorio<Alimento, int?>
    {
        public AlimentoRepositorio(FTContext db) : base(db)
        {
        }

       
    }
}
