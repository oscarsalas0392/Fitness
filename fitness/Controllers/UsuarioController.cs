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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Fitness.Controllers
{
    public class UsuarioController : Controller
    {
      
        private readonly FTContext _context;
        private readonly IRepositorio<Usuario, int?> _cR;
        private readonly IRepositorio<Genero, int?> _cRG;
        private readonly IRepositorio<TipoPeso, int?> _cRTP;
        private readonly IRepositorio<TipoAltura, int?> _cRTA;
        public UsuarioController(FTContext context, IRepositorio<Usuario, int?> cR, IRepositorio<Genero, int?> cRG, IRepositorio<TipoPeso, int?> cRTP, IRepositorio<TipoAltura, int?> cRTA)
        {
            _context = context;
            _cR = cR;
            _cRG = cRG;
            _cRTP = cRTP;
            _cRTA = cRTA;
        }

        public async Task<List<Genero>> obtenerListaGenero()
        {
            Notificacion<Genero> notificacion = await _cRG.ObtenerLista();
            if (notificacion is not null && notificacion.lista is not null)
            {
                return notificacion.lista.ToList();
            }
            return new List<Genero>();       
        }

        public async Task<List<TipoPeso>> obtenerListaPeso()
        {
            Notificacion<TipoPeso> notificacion = await _cRTP.ObtenerLista();
            if (notificacion is not null && notificacion.lista is not null)
            {
                return notificacion.lista.ToList();
            }

             return new List<TipoPeso>();
        }

        public async Task<List<TipoAltura>> obtenerListaAltura()
        {
            Notificacion<TipoAltura> notificacion = await _cRTA.ObtenerLista();
            if (notificacion is not null && notificacion.lista is not null)
            {
                return notificacion.lista.ToList();
            }

            return new List<TipoAltura>();
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
            viewModel.lstPeso = await obtenerListaPeso();
            viewModel.lstAltura = await obtenerListaAltura();
            return View(viewModel);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Correo,Nombre,FechaNacimiento,Altura,Peso,Genero,TipoPeso,TipoAltura,Foto")] Usuario usuario, IFormFile? photo= null)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }
            RegistrarViewModel viewModel = new RegistrarViewModel();
            viewModel.lstGenero = await obtenerListaGenero();
            viewModel.lstPeso = await obtenerListaPeso();
            viewModel.lstAltura = await obtenerListaAltura();

            string? json = HttpContext.Session.GetString("usuario");
            Usuario? usuarioSession = JsonConvert.DeserializeObject<Usuario>(json);

            if(photo == null) 
            {
                usuario.Foto = usuarioSession.Foto;
            }
            usuario.Contrasena = usuarioSession.Contrasena;
            ModelState.Remove("usuario.Contrasena");
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

                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(usuario));
                return await Edit(usuario.Id);
            }

            viewModel.usuario = usuario;
            return View(viewModel);
        }

        
    }
}
