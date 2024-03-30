using Fitness.Data.ClasesRepository;
using Fitness.Data.Interfaces;
using Fitness.Data;
using Fitness.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitness.ClasesBase;
using Fitness.Models;
using Fitness.Notificacion;

namespace Fitness.Controllers
{
    public class EstadisticaController : BaseController
    {

        private readonly FTContext _context;
        private readonly BitacoraPesoRepositorio _cR2;
        private readonly ActividadFisicaRepositorio _cRAF;
        private readonly MetaSaludRepositorio _cRMS;
        public EstadisticaController(FTContext context, BitacoraPesoRepositorio cR2, ActividadFisicaRepositorio cRAF, MetaSaludRepositorio cRMS)
        {
            _context = context;
            _cR2 = cR2;
            _cRAF = cRAF;
            _cRMS = cRMS;
        }
        public ActionResult Peso()
        {
            return View();
        }

        public ActionResult Calorias()
        {
            return View();
        }


        public async Task<IActionResult> Logros()
        {
            Filtro filtro = new Filtro();
            filtro.usuario = Usuario().Id;

            Notificacion<BitacoraPeso> notificacionPesos = await _cR2.ObtenerListaPesos(Usuario().Id);
            Notificacion<MetaSalud> notificacionMetaSalud = await _cRMS.ObtenerLista(filtro);

            List<MetaSalud> lstMetaSalud = notificacionMetaSalud.lista.ToList();
            List<BitacoraPeso> lstPesos = notificacionPesos.lista.ToList();


            //lstMetaSalud.Where(meta => lstPesos.Where(x=> x.Fecha == meta.FechaObjectivo).FirstOrDefault().Peso <=  )

            return View();
        }

        public async Task<GraficoPeso> ObtenerDatosPeso() 
        {

            Notificacion<BitacoraPeso> notificacion = await _cR2.ObtenerListaPesos(Usuario().Id);

            if (!notificacion._estado || notificacion._excepcion)
            { 
                return new GraficoPeso();
            }
            GraficoPeso graficoPeso= new GraficoPeso();
            graficoPeso.historialPeso = notificacion.lista.Select(x => x.Peso).ToList();
            graficoPeso.historialFecha = notificacion.lista.Select(x => x.Fecha.ToString("dd-MM-yyyy hh:mm tt")).ToList();

            return graficoPeso;
            
        }

        public async Task<GraficoCalorias> ObtenerCalorias()
        {
            Filtro filtro = new Filtro();
            filtro.usuario = Usuario().Id;

            Notificacion<ActividadFisica> notificacion = await _cRAF.ObtenerLista();

            if (!notificacion._estado || notificacion._excepcion)
            {
                return new GraficoCalorias();
            }


            GraficoCalorias graficoCalorias= new GraficoCalorias();
            graficoCalorias.historialCalorias = notificacion.lista
                                                             .GroupBy(x => x.Fecha.ToString("dd-MM-yyyy"))
                                                             .Select(x=> x.Sum(c=>c.Calorias))
                                                             .ToList();

            graficoCalorias.historialFecha = notificacion.lista.GroupBy(x => x.Fecha.ToString("dd-MM-yyyy")).Select(x => x.First().Fecha.ToString("dd-MM-yyyy")).ToList();

            return graficoCalorias;

        }


    }
}
