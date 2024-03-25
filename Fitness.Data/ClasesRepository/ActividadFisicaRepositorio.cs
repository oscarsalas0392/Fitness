﻿using Fitness.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class ActividadFisicaRepositorio : BaseRepositorio<ActividadFisica, int?>
    {
        public ActividadFisicaRepositorio(FTContext db) : base(db){}
    }
}
