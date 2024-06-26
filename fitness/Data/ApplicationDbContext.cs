﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fitness.Model.Models;

namespace Fitness.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<Fitness.Model.Models.ActividadFisica> ActividadFisica { get; set; } = default!;
        public DbSet<Fitness.Model.Models.TipoComida> TipoComida { get; set; } = default!;
        public DbSet<Fitness.Model.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<Fitness.Model.Models.Dieta> Dieta { get; set; } = default!;
        public DbSet<Fitness.Model.Models.AlimentoConsumido> AlimentoConsumido { get; set; } = default!;
        public DbSet<Fitness.Model.Models.MetaSalud> MetaSalud { get; set; } = default!;
        public DbSet<Fitness.Model.Models.Alimento> Alimento { get; set; } = default!;
    }
}
