using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Data;
using Fitness.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitness.Notificacion;


namespace Fitness.Controllers
{
    public class AccesoController : Controller
    {

        private readonly FTContext _context;
        private readonly IRepositorio<Usuario, int?> _cR;
        private readonly UsuarioRepositorio _cR2;

        public AccesoController(FTContext context, IRepositorio<Usuario, int?> cR, UsuarioRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }
        public IActionResult Login()
        {
            int? id = HttpContext.Session.GetInt32("usuario");
            if (id != null)
            { 
               return RedirectToAction("Index","Home");
            }
            return View();
        }


        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async  Task<ActionResult> Login(Usuario oUsuario)
        {

           Notificacion<Usuario> notificacion =  await _cR2.ValidarUsuario(oUsuario.Correo,oUsuario.NombreUsuario,oUsuario.Contrasena);
           Usuario? usuario = notificacion.objecto;

         if(usuario is null)
         {
              TempData["MensajeError"]= "El usuario o la contraseña no son correctos";
         }

         else 
         {
                HttpContext.Session.SetInt32("usuario", usuario.Id);
                return RedirectToAction("Index", "Home");
         }

           return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            HttpContext.Session.SetInt32("usuario", oUsuario.Id);
            return View();
        }
    }
}
