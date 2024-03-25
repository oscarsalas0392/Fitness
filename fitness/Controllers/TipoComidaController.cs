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
using Fitness.ViewModels;
using Fitness.Extensions;

namespace Fitness.Controllers
{
    public class TipoComidaController : Controller
    {
        private readonly FTContext _context;
        private readonly IRepositorio<TipoComida, int?> _cR;
        private readonly TipoComidaRepositorio _cR2;

        public TipoComidaController(FTContext context, IRepositorio<TipoComida, int?> cR, TipoComidaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: TipoComida
        public async Task<IActionResult> Index(TipoComidaIndexViewModel vm)
        {
            await vm.HandleRequest(_cR2);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
          
        }

        // GET: TipoComida/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
  
            return View(notificacion.objecto);
        }

        // GET: TipoComida/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoComida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Calorias,Eliminado")] TipoComida tipoComida)
        {
            if (ModelState.IsValid)
            {
                Notificacion<TipoComida> notificacion = await _cR.Guardar(tipoComida);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoComida);
        }

        // GET: TipoComida/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // POST: TipoComida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Calorias,Eliminado")] TipoComida tipoComida)
        {
            if (id != tipoComida.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Notificacion<TipoComida> notificacion = await _cR.Actualizar(tipoComida);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(tipoComida);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoComida);
        }

        // GET: TipoComida/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: TipoComida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<TipoComida> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

 
    }
}
