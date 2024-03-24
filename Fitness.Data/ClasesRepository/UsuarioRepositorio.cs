using Fitness.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Data.ClasesRepository
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario, int>
    {
        public UsuarioRepositorio(FTContext db) : base(db){}

        public async Task<bool> ValidarUsuario(string correo, string nombreUsuario, string contrasena)
        {

           Usuario? usuario =  await _db.Usuarios.Where(x => (x.Correo == correo || x.NombreUsuario == nombreUsuario) && x.Contrasena == contrasena).FirstOrDefaultAsync();
           bool blnResultado = usuario is Usuario ? true : false;
           return blnResultado;
        }
    }
}
