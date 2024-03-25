using Fitness.Data;
using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Model.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fitness
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<FTContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PR")));

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("PR")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
           .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<IRepositorio<Dieta, int?>, DietaRepositorio>();
            services.AddTransient<IRepositorio<Genero, int?>, GeneroRepositorio>();
            services.AddTransient<IRepositorio<MetaSalud, int?>, MetaSaludRepositorio>();
            services.AddTransient<IRepositorio<TipoAltura, int?>, TipoAlturaRepositorio>();
            services.AddTransient<IRepositorio<TipoComida, int?>, TipoComidaRepositorio>();
            services.AddTransient<IRepositorio<TipoDistancia, int?>, TipoDistanciaRepositorio>();
            services.AddTransient<IRepositorio<TipoMeta, int?>, TipoMetaRepositorio>();
            services.AddTransient<IRepositorio<TipoPeso, int?>, TipoPesoRepositorio>();
            services.AddTransient<IRepositorio<Usuario, int?>, UsuarioRepositorio>();
            services.AddTransient<IRepositorio<TipoActividadFisica, int?>, TipoActividadFisicaRepositorio>();
            services.AddTransient<IRepositorio<ActividadFisica, int?>, ActividadFisicaRepositorio>();

            services.AddTransient(typeof(DietaRepositorio));
            services.AddTransient(typeof(GeneroRepositorio));
            services.AddTransient(typeof(MetaSaludRepositorio));
            services.AddTransient(typeof(TipoAlturaRepositorio));
            services.AddTransient(typeof(TipoComidaRepositorio));
            services.AddTransient(typeof(TipoDistanciaRepositorio));
            services.AddTransient(typeof(TipoMetaRepositorio));
            services.AddTransient(typeof(TipoPesoRepositorio));
            services.AddTransient(typeof(UsuarioRepositorio));
            services.AddTransient(typeof(TipoActividadFisicaRepositorio));
            services.AddTransient(typeof(ActividadFisicaRepositorio));


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
           
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

    }
}
