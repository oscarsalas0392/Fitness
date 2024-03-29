using Fitness.Enums;
using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.AspNetCore.Http;
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

        public int Dieta()
        {
            int? dieta = HttpContext.Session.GetInt32(SessionKey.dieta.ToString());
            return dieta is null ? 0 : dieta.Value;
        }

        public int Opcion() 
        {
            int? dieta = HttpContext.Session.GetInt32(SessionKey.opcion.ToString());
            return dieta is null ? 0 : dieta.Value;
        }

        public void GuardarIntSession(string key, int valor)
        {
           HttpContext.Session.SetInt32(key, valor);
        }
        public void GuardarOjecto(string key, object ojecto)
        {
           string json =  JsonConvert.SerializeObject(ojecto);
           HttpContext.Session.SetString(key, json);
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
