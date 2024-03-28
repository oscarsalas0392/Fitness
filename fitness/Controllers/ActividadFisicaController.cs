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
using Fitness.Notificacion;
using Fitness.ClasesBase;

namespace Fitness.Controllers
{
    public class ActividadFisicaController : BaseController
    {
        private readonly FTContext _context;
        private readonly IRepositorio<ActividadFisica, int?> _cR;
        private readonly ActividadFisicaRepositorio _cR2;

        public ActividadFisicaController(FTContext context, IRepositorio<ActividadFisica, int?> cR, ActividadFisicaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: ActividadFisica
        public async Task<IActionResult> Index(IndexViewModel<ActividadFisica, ActividadFisicaRepositorio, int?> vm)
        {
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion");
            await vm.HandleRequest(_cR2,"Fecha", "TipoActividadFisica", Usuario().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: ActividadFisica/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id);
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion");
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Descripcion");
            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // GET: ActividadFisica/Create
        public IActionResult Create()
        {
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion");
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Descripcion");
            return View();
        }

        // POST: ActividadFisica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoActividadFisica,Fecha,Duracion,Distancia,TipoDistancia,Calorias,Comentarios")] ActividadFisica actividadFisica)
        {
            actividadFisica.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<ActividadFisica> notificacion = await _cR.Guardar(actividadFisica);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion", actividadFisica.TipoActividadFisica);
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Descripcion", actividadFisica.TipoDistancia);
          
            return View(actividadFisica);
        }

        // GET: ActividadFisica/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion", notificacion.objecto.TipoActividadFisica);
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Descripcion", notificacion.objecto.TipoDistancia);
            return View(notificacion.objecto);
        }

        // POST: ActividadFisica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoActividadFisica,Fecha,Duracion,Distancia,TipoDistancia,Calorias,Comentarios")] ActividadFisica actividadFisica)
        {
            if (id != actividadFisica.Id)
            {
                return NotFound();
            }
            actividadFisica.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<ActividadFisica> notificacion = await _cR.Actualizar(actividadFisica);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(actividadFisica);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Id", actividadFisica.TipoActividadFisica);
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Id", actividadFisica.TipoDistancia);
   
            return View(actividadFisica);
        }

        // GET: ActividadFisica/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id);
            ViewData["TipoActividadFisica"] = new SelectList(_context.Set<TipoActividadFisica>(), "Id", "Descripcion", notificacion.objecto.TipoActividadFisica);
            ViewData["TipoDistancia"] = new SelectList(_context.Set<TipoDistancia>(), "Id", "Descripcion");
            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: ActividadFisica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<ActividadFisica> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
