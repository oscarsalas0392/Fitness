﻿using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class MetaSaludRepositorio : BaseRepositorio<MetaSalud, int?>
    {
        public MetaSaludRepositorio(FTContext db) : base(db){
            lstIncludes.Add("TipoMetaNavigation");
            lstIncludes.Add("TipoPesoNavigation");
        }
    }
}
