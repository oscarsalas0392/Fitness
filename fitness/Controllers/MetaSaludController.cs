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

namespace Fitness.Controllers
{
    public class MetaSaludController : BaseController
    {
        private readonly FTContext _context;
        private readonly IRepositorio<MetaSalud, int?> _cR;
        private readonly MetaSaludRepositorio _cR2;
        private readonly TipoMetaRepositorio _cRTM; 
        private readonly TipoPesoRepositorio _cRTP;
        public MetaSaludController(FTContext context, IRepositorio<MetaSalud, int?> cR, MetaSaludRepositorio cR2, TipoMetaRepositorio cRTM, TipoPesoRepositorio cRTP)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
            _cRTM = cRTM;
            _cRTP = cRTP;
        }

        // GET: MetaSalud
        public async Task<IActionResult> Index(IndexViewModel<MetaSalud, MetaSaludRepositorio, int?> vm)
        {
            await vm.HandleRequest(_cR2, "FechaObjectivo", "TipoMetaNavigation.Descripcion", Usuario().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: MetaSalud/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Notificacion<MetaSalud> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // GET: MetaSalud/Create
        public async Task<IActionResult> Create()
        {
            Notificacion<TipoMeta> notificacionTipoMeta = await _cRTM.ObtenerLista();
            Notificacion<TipoPeso> notificacionTipoPeso = await _cRTP.ObtenerLista();
            ViewData["TipoMeta"] = new SelectList(notificacionTipoMeta.lista, "Id", "Descripcion");
            ViewData["TipoPeso"] = new SelectList(notificacionTipoPeso.lista, "Id", "Descripcion"); 
            return View();
        }

        // POST: MetaSalud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usuario,TipoMeta,PesoObjectivo,TipoPeso,FechaObjectivo,NivelActividad,OjectivoEspecifico,Eliminado")] MetaSalud metaSalud)
        {

            metaSalud.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<MetaSalud> notificacion = await _cR.Guardar(metaSalud);
                return RedirectToAction(nameof(Index));
            }

            Notificacion<TipoMeta> notificacionTipoMeta = await _cRTM.ObtenerLista();
            Notificacion<TipoPeso> notificacionTipoPeso = await _cRTP.ObtenerLista();
            ViewData["TipoMeta"] = new SelectList(notificacionTipoMeta.lista, "Id", "Descripcion", metaSalud.TipoMeta);
            ViewData["TipoPeso"] = new SelectList(notificacionTipoPeso.lista, "Id", "Descripcion", metaSalud.TipoPeso);  
            return View(metaSalud);
        }

        // GET: MetaSalud/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<MetaSalud> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }

            Notificacion<TipoMeta> notificacionTipoMeta = await _cRTM.ObtenerLista();
            Notificacion<TipoPeso> notificacionTipoPeso = await _cRTP.ObtenerLista();
            ViewData["TipoMeta"] = new SelectList(notificacionTipoMeta.lista, "Id", "Descripcion", notificacion.objecto.TipoMeta);
            ViewData["TipoPeso"] = new SelectList(notificacionTipoPeso.lista, "Id", "Descripcion", notificacion.objecto.TipoPeso);
            return View(notificacion.objecto);
        }

        // POST: MetaSalud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,TipoMeta,PesoObjectivo,TipoPeso,FechaObjectivo,NivelActividad,OjectivoEspecifico,Eliminado")] MetaSalud metaSalud)
        {
            if (id != metaSalud.Id)
            {
                return NotFound();
            }
            metaSalud.Usuario = Usuario().Id;
            if (ModelState.IsValid)
            {
                Notificacion<MetaSalud> notificacion = await _cR.Actualizar(metaSalud);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(metaSalud);
                }
                return RedirectToAction(nameof(Index));
            }
            Notificacion<TipoMeta> notificacionTipoMeta = await _cRTM.ObtenerLista();
            Notificacion<TipoPeso> notificacionTipoPeso = await _cRTP.ObtenerLista();
            ViewData["TipoMeta"] = new SelectList(notificacionTipoMeta.lista, "Id", "Descripcion", metaSalud.TipoMeta);
            ViewData["TipoPeso"] = new SelectList(notificacionTipoPeso.lista, "Id", "Descripcion", metaSalud.TipoPeso);
          
            return View(metaSalud);
        }

        // GET: MetaSalud/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Notificacion<MetaSalud> notificacion = await _cR.ObtenerId(id);
            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: MetaSalud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<MetaSalud> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

    
    }
}
