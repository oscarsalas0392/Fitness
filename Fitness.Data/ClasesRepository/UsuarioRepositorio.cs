using Fitness.Model.Models;
using Fitness.Notificacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario, int?>
    {
        public UsuarioRepositorio(FTContext db) : base(db){}

        public async Task<Notificacion<Usuario>> ValidarUsuario(string correo, string nombreUsuario, string contrasena)
        {
            try 
            {
                if (_db is null) { return new Notificacion<Usuario>(true, Accion.obtenerLista, true); }
                Usuario? usuario = await _db.Usuario.Where(x => (x.Correo == correo || x.NombreUsuario == nombreUsuario) && x.Contrasena == contrasena).FirstOrDefaultAsync();
                Notificacion<Usuario> notificacion = new Notificacion<Usuario>(true,Accion.obtener);
                notificacion.objecto = usuario;
                return notificacion;
            }
            catch 
            {
                Notificacion<Usuario> notificacion = new Notificacion<Usuario>(true, Accion.obtener, true);
                notificacion.objecto = null;
                return notificacion;
            }
         
        }
    }
}
