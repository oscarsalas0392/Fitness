using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    }
}
