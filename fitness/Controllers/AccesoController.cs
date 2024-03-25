using Fitness.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Fitness.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
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
        public ActionResult Login(Usuario oUsuario)
        {
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
