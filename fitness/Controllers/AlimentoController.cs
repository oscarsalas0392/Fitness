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

namespace Fitness.Controllers
{
    public class AlimentoController : Controller
    {
        private readonly FTContext _context;
        private readonly IRepositorio<Alimento, int?> _cR;
        private readonly AlimentoRepositorio _cR2;


        public AlimentoController(FTContext context, IRepositorio<Alimento, int?> cR, AlimentoRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: Alimentoes
        public async Task<IActionResult> Index(IndexViewModel<Alimento, AlimentoRepositorio, int?> vm)
        {
            await vm.HandleRequest(_cR2, "Descripcion", "Descripcion");

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: Alimentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Notificacion<Alimento> notificacion = await _cR.ObtenerId(id);
            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }

        // GET: Alimentoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alimentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Calorias,Eliminado")] Alimento alimento)
        {
      
            if (ModelState.IsValid)
            {
                Notificacion<Alimento> notificacion = await _cR.Guardar(alimento);
                return RedirectToAction(nameof(Index));
            }
            
            return View(alimento);
        }

        // GET: Alimentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<Alimento> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: Alimentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Calorias,Eliminado")] Alimento alimento)
        {
            if (id != alimento.Id)
            {
                return NotFound();
            }
     
            if (ModelState.IsValid)
            {
                Notificacion<Alimento> notificacion = await _cR.Actualizar(alimento);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(alimento);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alimento);
        }

        // GET: Alimentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Notificacion<Alimento> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: Alimentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<Alimento> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
