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
    public class TipoComidaController : Controller
    {
        private readonly FTContext _context;
        private readonly IRepositorio<TipoComida, int> _cR;
        private readonly TipoComidaRepositorio _cR2;


        public TipoComidaController(FTContext context, IRepositorio<TipoComida, int> cR, TipoComidaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: TipoComida
        public async Task<IActionResult> Index()
        {
            Notificacion<TipoComida> notificacion = await _cR.ObtenerLista();
            return View(notificacion.lista);
        }

        // GET: TipoComida/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            TipoComida? tipoComida = notificacion.objecto;

            return View(tipoComida);
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
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] TipoComida tipoComida)
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
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            TipoComida? tipoComida = notificacion.objecto;

            return View(tipoComida);
        }

        // POST: TipoComida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] TipoComida tipoComida)
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
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<TipoComida> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            TipoComida? tipoComida = notificacion.objecto;

            return View(tipoComida);
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
