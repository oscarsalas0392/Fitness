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
using Fitness.Data.Clases;
using Fitness.Notificacion;
using Fitness.Enums;

namespace Fitness.Controllers
{
    public class AlimentoConsumidoController : BaseController
    {

        private readonly FTContext _context;
        private readonly IRepositorio<AlimentoConsumido, int?> _cR;
        private readonly AlimentoConsumidoRepositorio _cR2;
        private readonly DietaRepositorio _cRD;
        private readonly AlimentoRepositorio _cRA;


        public AlimentoConsumidoController(FTContext context, IRepositorio<AlimentoConsumido, int?> cR, AlimentoConsumidoRepositorio cR2, DietaRepositorio cRD, AlimentoRepositorio cRA)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
            _cRD = cRD;
            _cRA = cRA;
        }

 

        // GET: AlimentoConsumido
        public async Task<IActionResult> Index(IndexViewModel<AlimentoConsumido, AlimentoConsumidoRepositorio, int?> vm)
        {
            int? opcion = !Request.IsAjaxRequest() ? 1 : vm.paginacion.opcionGrupo;
            await vm.HandleRequest(_cR2, "AlimentoNavigation.Descripcion", "AlimentoNavigation.Descripcion",Dieta(), opcion.Value);

            Notificacion<Dieta> notificacionDieta = await _cRD.ObtenerId(Dieta());
            if (notificacionDieta._estado && !notificacionDieta._excepcion && notificacionDieta.objecto is not null)
            {
                ViewBag.IdDieta = Dieta();
                ViewBag.TipoComida = notificacionDieta.objecto.TipoComidaNavigation.Descripcion;              
            }

            Notificacion<Opcion> notificacionOpcion = await _cR2.ObtenerOpciones(Dieta());
            Notificacion<int> notificacionTotalCalorias = _cR2.TotalCalorias(vm.paginacion);

            ViewBag.TotalCalorias = notificacionTotalCalorias.objecto;
            ViewData["Opciones"] = new SelectList(notificacionOpcion.lista, "Id", "Id");
 
            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: AlimentoConsumido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Notificacion<AlimentoConsumido> notificacion = await _cR.ObtenerId(id);
           
            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
     
            return View(notificacion.objecto);
        }

        // GET: AlimentoConsumido/Create
        public async Task<IActionResult> Create(int idConsumido)
        {
            Notificacion<Alimento> notificacionAlimento = await _cRA.ObtenerLista();
            ViewData["Alimento"] = new SelectList(notificacionAlimento.lista, "Id", "Descripcion");
            Notificacion<Opcion> notificacionOpcion =  _cR2.ObtenerOpciones();
            ViewData["Opcion"] = new SelectList(notificacionOpcion.lista, "Id", "Id");
            return View();
        }

        // POST: AlimentoConsumido/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Alimento,Opcion")] AlimentoConsumido alimentoConsumido)
        {
            alimentoConsumido.Dieta = Dieta();
            if (ModelState.IsValid)
            {
                Notificacion<AlimentoConsumido> notificacion = await _cR.Guardar(alimentoConsumido);
                return RedirectToAction(nameof(Index));
            }
            Notificacion<Alimento> notificacionAlimento = await _cRA.ObtenerLista();


            ViewData["Alimento"] = new SelectList(notificacionAlimento.lista, "Id", "Descripcion", alimentoConsumido.Alimento);
            return View(alimentoConsumido);
        }

        // GET: AlimentoConsumido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Notificacion<AlimentoConsumido> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
            Notificacion<Alimento> notificacionAlimento = await _cRA.ObtenerLista();
            ViewData["Alimento"] = new SelectList(notificacionAlimento.lista, "Id", "Descripcion");
            Notificacion<Opcion> notificacionOpcion =  _cR2.ObtenerOpciones();
            ViewData["Opcion"] = new SelectList(notificacionOpcion.lista, "Id", "Id");
    
            return View(notificacion.objecto);
        }

        // POST: AlimentoConsumido/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Alimento,Opcion")] AlimentoConsumido alimentoConsumido)
        {
            if (id != alimentoConsumido.Id)
            {
                return NotFound();
            }
            alimentoConsumido.Dieta = Dieta();


            if (ModelState.IsValid)
            {
                Notificacion<AlimentoConsumido> notificacion = await _cR.Actualizar(alimentoConsumido);

                if (!notificacion._estado || notificacion._excepcion)
                {
                    Mensaje mensaje = notificacion.mensaje;
                    ModelState.AddModelError("", $"{mensaje.titulo} - {mensaje.Descripcion}");
                    return View(alimentoConsumido);
                }
                return RedirectToAction(nameof(Index));
            }
            Notificacion<Alimento> notificacionAlimento = await _cRA.ObtenerLista();
            ViewData["Alimento"] = new SelectList(notificacionAlimento.lista, "Id", "Descripcion", alimentoConsumido.Alimento);      
            return View(alimentoConsumido);
        }

        // GET: AlimentoConsumido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Notificacion<AlimentoConsumido> notificacion = await _cR.ObtenerId(id);
     
            if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
            {
                return NotFound();
            }
       
            return View(notificacion.objecto);
        
        }

        // POST: AlimentoConsumido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Notificacion<AlimentoConsumido> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

 
    }
}
