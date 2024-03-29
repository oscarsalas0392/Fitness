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
using Fitness.Extensions;
using Fitness.ViewModels;
using Fitness.ClasesBase;
using Fitness.Notificacion;
using Fitness.Enums;

namespace Fitness.Controllers
{
    public class DietaController : BaseController
    {
        private readonly FTContext _context;
        private readonly IRepositorio<Dieta, int?> _cR;
        private readonly DietaRepositorio _cR2; 

        public DietaController(FTContext context, IRepositorio<Dieta, int?> cR, DietaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: Dieta
        public async Task<IActionResult> Index(IndexViewModel<Dieta, DietaRepositorio, int?> vm)
        {
            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion");
            
            await vm.HandleRequest(_cR2, "Fecha", "TipoComida", Usuario().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: Dieta/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion");
            Notificacion<Dieta> notificacion = await _cR.ObtenerId(id);
            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // GET: Dieta/Create
        public IActionResult Create()
        {
            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion");
            return View();
        }

        // POST: Dieta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usuario,Fecha,TipoComida,Calorias,Comentarios,Eliminado")] Dieta dieta)
        {
            dieta.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<Dieta> notificacion = await _cR.Guardar(dieta);
                if (notificacion._estado && !notificacion._excepcion && notificacion.objecto is not null)
                {
                    GuardarIntSession(SessionKey.dieta.ToString(), notificacion.objecto.Id);
                    return RedirectToAction("Index", "AlimentoConsumido");
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion", dieta.TipoComida);
            return View(dieta);
        }

        // GET: Dieta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<Dieta> notificacion = await _cR.ObtenerId(id);
          
            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            GuardarIntSession(SessionKey.dieta.ToString(), notificacion.objecto.Id);
            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion", notificacion.objecto.TipoComida);
 
            return View(notificacion.objecto);
        }

        // POST: Dieta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Fecha,TipoComida,Calorias,Comentarios,Eliminado")] Dieta dieta)
        {
            if (id != dieta.Id)
            {
                return NotFound();
            }
            dieta.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<Dieta> notificacion = await _cR.Actualizar(dieta);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(dieta);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion", dieta.TipoComida);
            return View(dieta);
        }

        // GET: Dieta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            Notificacion<Dieta> notificacion = await _cR.ObtenerId(id);
            ViewData["TipoComida"] = new SelectList(_context.TipoComida, "Id", "Descripcion", notificacion?.objecto?.TipoComida);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // POST: Dieta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<Dieta> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
