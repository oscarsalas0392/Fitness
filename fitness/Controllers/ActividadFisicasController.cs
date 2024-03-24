using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fitness.Data;
using Fitness.Model.Models;
using Fitness.Data.Interfaces;
using Fitness.Data.ClasesRepository;
using Fitness.Notificacion;

namespace Fitness.Controllers
{
    public class ActividadFisicasController : Controller
    {
        private readonly FTContext _context;
        private readonly IRepositorio<ActividadFisica, int> _cR;
        private readonly ActividadFisicaRepositorio _cR2;

        public ActividadFisicasController(FTContext context, IRepositorio<ActividadFisica, int> cR, ActividadFisicaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: ActividadFisicas
        public async Task<IActionResult> Index()
        {

            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerLista();
            return View(notificacion.lista);
        }

        // GET: ActividadFisicas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            ActividadFisica? actividadFisica =notificacion.objecto;

            return View(actividadFisica);
        }

        // GET: ActividadFisicas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActividadFisicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usuario,Tipo,Fecha,Duracion,Distancia,TipoDistancia,Calorias,Comentarios")] ActividadFisica actividadFisica)
        {
            if (ModelState.IsValid)
            {
                Notificacion<ActividadFisica> notificacion = await _cR.Guardar(actividadFisica);
                return RedirectToAction(nameof(Index));
            }
            return View(actividadFisica);
        }

        // GET: ActividadFisicas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            ActividadFisica? actividadFisica = notificacion.objecto;

            return View(actividadFisica);
        }

        // POST: ActividadFisicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Tipo,Fecha,Duracion,Distancia,TipoDistancia,Calorias,Comentarios")] ActividadFisica actividadFisica)
        {
            if (id != actividadFisica.Id)
            {
                return NotFound();
            }

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
            return View(actividadFisica);
        }

        // GET: ActividadFisicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notificacion<ActividadFisica> notificacion = await _cR.ObtenerId(id.Value);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            ActividadFisica? actividadFisica = notificacion.objecto;

            return View(actividadFisica);
        }

        // POST: ActividadFisicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<ActividadFisica> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

   
    }
}
