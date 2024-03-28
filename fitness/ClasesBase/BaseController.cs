using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Fitness.ClasesBase
{
    public class BaseController :  Controller
    {
        public Usuario Usuario() 
        {
            string? json = HttpContext.Session.GetString("usuario");

            if (json != null)
            {
                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

                if (usuario is null)
                {
                    return new Usuario();
                }
                return usuario;
            }
            return new Usuario();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string? json = HttpContext.Session.GetString("usuario");
                if (json == null)
                {
                    context.Result = RedirectToAction("Index", "Acceso");
                    return;
                }

                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

                if (usuario is null)
                {
                    context.Result = RedirectToAction("Index", "Acceso");
                }
            }
            catch 
            {
                context.Result = RedirectToAction("Index", "Acceso");
            }
        }
    }
}
