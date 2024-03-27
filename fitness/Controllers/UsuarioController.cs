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
using Newtonsoft.Json;
using Fitness.ViewModels;

namespace Fitness.Controllers
{
    public class UsuarioController : Controller
    {
      
        private readonly FTContext _context;
        private readonly IRepositorio<Usuario, int?> _cR;
        private readonly IRepositorio<Genero, int?> _cRG;

        public UsuarioController(FTContext context, IRepositorio<Usuario, int?> cR, IRepositorio<Genero, int?> cRG)
        {
            _context = context;
            _cR = cR;
            _cRG = cRG;
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
        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<Usuario> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            RegistrarViewModel viewModel = new RegistrarViewModel();
            viewModel.usuario = notificacion.objecto;
            viewModel.lstGenero = await obtenerListaGenero();

            return View(viewModel);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Correo,Nombre,FechaNacimiento,Altura,Peso,Genero,Foto")] Usuario usuario, IFormFile? photo= null)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }
            RegistrarViewModel viewModel = new RegistrarViewModel();
            viewModel.lstGenero = await obtenerListaGenero();
            string? json = HttpContext.Session.GetString("usuario");
            Usuario? usuarioSession = JsonConvert.DeserializeObject<Usuario>(json);

            if(photo == null) 
            {
                usuario.Foto = usuarioSession.Foto;
            }
            
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    
                    using (MemoryStream ms = new MemoryStream())
                    {
                        photo.CopyTo(ms);
                        string base64String = $"data:{photo.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
                        usuario.Foto = base64String;
                    }
              
                }

               
                usuario.Contrasena = usuarioSession.Contrasena;
                usuario.Eliminado = false;
                usuario.Id = usuarioSession.Id;

                Notificacion<Usuario> notificacion = await _cR.Actualizar(usuario);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    viewModel.usuario = usuario;
                    return View(viewModel);
                }
                return await Edit(usuario.Id);
            }

            viewModel.usuario = usuario;
            return View(viewModel);
        }

        
    }
}
