using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Data;
using Fitness.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitness.Notificacion;
using Fitness.ViewModels;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Fitness.Controllers
{
    public class AccesoController : Controller
    {

        private readonly FTContext _context;
        private readonly UsuarioRepositorio _cRU;
        private readonly IRepositorio<Genero, int?> _cRG;
        private readonly IRepositorio<TipoPeso, int?> _cRP;
        private readonly IRepositorio<TipoAltura, int?> _cRA;
    
        public AccesoController(FTContext context, UsuarioRepositorio cRU, IRepositorio<Genero, int?> cRG, IRepositorio<TipoPeso, int?> cRP, IRepositorio<TipoAltura, int?> cRA)
        {
            _context = context;
            _cRU = cRU;
            _cRG= cRG;
            _cRP= cRP;
            _cRA= cRA;
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
        public async Task<List<Genero>> obtenerListaGenero()
        {
            Notificacion<Genero> notiGenero = await _cRG.ObtenerLista();

            if (notiGenero is not null && notiGenero.lista is not null)
            {
                return notiGenero.lista.ToList();
            }
            else 
            { 
                return new List<Genero>();
            }
  
        }
        public async Task<ActionResult> Registrar()
        {
            RegistrarViewModel registrarViewModel = new RegistrarViewModel();
            registrarViewModel.lstGenero = await obtenerListaGenero();
            return View(registrarViewModel);
        }

        [HttpPost]
        public async  Task<ActionResult> Login(Usuario oUsuario)
        {
           Notificacion<Usuario> notificacion =  await _cRU.ValidarUsuario(oUsuario.Correo,oUsuario.NombreUsuario,oUsuario.Contrasena);
           Usuario? usuario = notificacion.objecto;

         if(usuario is null)
         {
              TempData["MensajeError"]= "El usuario o la contraseña no son correctos";
         }

         else
         {

                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(notificacion.objecto));
                return RedirectToAction("Index", "Home");
         }

           return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(Usuario oUsuario)
        {
            Notificacion<Usuario> notificacion = await _cRU.Guardar(oUsuario);

            if (!notificacion._estado ||  notificacion._excepcion || notificacion.objecto is null)
            {
                TempData["MensajeError"] = notificacion.mensaje.Descripcion;
            }
            else 
            {

                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(notificacion.objecto));
                return RedirectToAction("Index", "Home");
            }

            RegistrarViewModel registrarViewModel = new RegistrarViewModel();
            registrarViewModel.lstGenero = await obtenerListaGenero();
            registrarViewModel.usuario = oUsuario;
            return View(registrarViewModel);
        }

        
    }
}
