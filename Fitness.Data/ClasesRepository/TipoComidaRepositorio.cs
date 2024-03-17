using Fitness.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class TipoComidaRepositorio : BaseRepositorio<TipoComida, int>
    {
        public TipoComidaRepositorio(dbContext db) : base(db){}
    }
}
