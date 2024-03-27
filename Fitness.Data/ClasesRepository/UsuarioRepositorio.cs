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
                contrasena = DecryptAndEncrypt.EncryptStringAES(contrasena);
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
        public override async Task<Notificacion<Usuario>> Guardar(Usuario model)
        {
            Usuario usuario = new Usuario() 
            {
                NombreUsuario = model.NombreUsuario,
                Correo = model.Correo,
                Contrasena = DecryptAndEncrypt.EncryptStringAES(model.Contrasena),
                Nombre = model.Nombre,
                FechaNacimiento = model.FechaNacimiento,
                Altura= 0,
                TipoAltura = 1,
                TipoPeso = 1,
                Peso =  0,
                Genero = model.Genero

            };
          
            List<Usuario> validaciones = _db.Usuario.Where(x => x.Correo == model.Correo || x.NombreUsuario == model.NombreUsuario).ToList();
            Notificacion<Usuario> notificacion = new Notificacion<Usuario>(false,Accion.agregar);

            if (validaciones.Exists(x => x.Correo == model.Correo))
            {
                notificacion.mensaje = Mensajes.EXISTS_EMAIL;
                return notificacion;
            }
            if (validaciones.Exists(x => x.NombreUsuario == model.NombreUsuario))
            {
                notificacion.mensaje = Mensajes.EXISTS_USERNAME;
                return notificacion;
            }

            return await base.Guardar(usuario);
        }
    }
}
