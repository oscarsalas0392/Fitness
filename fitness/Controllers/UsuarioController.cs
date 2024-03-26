using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fitness.Data;
using Fitness.Model.Models;
using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Notificacion;

namespace Fitness.Controllers
{
    public class UsuarioController : Controller
    {
      
        private readonly FTContext _context;
        private readonly IRepositorio<Usuario, int?> _cR;
        
        public UsuarioController(FTContext context, IRepositorio<Usuario, int?> cR)
        {
            _context = context;
            _cR = cR;
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<Usuario> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Correo,Contrasena,Nombre,FechaNacimiento,Altura,Peso,Genero,Foto,Eliminado")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Notificacion<Usuario> notificacion = await _cR.Actualizar(usuario);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        
    }
}
