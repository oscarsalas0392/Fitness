using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fitness.ClasesBase
{
    public class BaseController :  Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? id = HttpContext.Session.GetInt32("usuario");
            if (id is null)
            {
                context.Result = RedirectToAction("Index", "Acceso");
            }
        }
    }
}
