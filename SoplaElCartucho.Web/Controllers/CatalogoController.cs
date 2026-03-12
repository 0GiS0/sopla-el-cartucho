using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SoplaElCartucho.Business.Services;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Web.Controllers
{
    /// <summary>
    /// Controlador para el catálogo de juegos y consolas.
    /// ⚠️ ANTI-PATRÓN LEGACY: Instanciación directa del servicio sin DI.
    /// 📝 MIGRACIÓN: Usar inyección de dependencias en constructor.
    /// </summary>
    public class CatalogoController : Controller
    {
        private readonly CatalogoService _catalogoService;
        private const int ITEMS_POR_PAGINA = 8;

        public CatalogoController()
        {
            _catalogoService = new CatalogoService();
        }

        public ActionResult Index(int? consolaId, string genero, string orden, int? pagina)
        {
            // Obtener datos desde la base de datos
            var todosLosJuegos = _catalogoService.ObtenerJuegos();
            var todasLasConsolas = _catalogoService.ObtenerConsolasActivas();

            // Filtrar juegos activos
            var juegos = todosLosJuegos.Where(j => j.Activo);

            if (consolaId.HasValue)
            {
                juegos = juegos.Where(j => j.ConsolaId == consolaId.Value);
                ViewBag.ConsolaActual = todasLasConsolas.FirstOrDefault(c => c.Id == consolaId.Value);
            }

            if (!string.IsNullOrEmpty(genero))
            {
                juegos = juegos.Where(j => j.Genero == genero);
            }

            switch (orden)
            {
                case "precio-asc":
                    juegos = juegos.OrderBy(j => j.Precio);
                    break;
                case "precio-desc":
                    juegos = juegos.OrderByDescending(j => j.Precio);
                    break;
                case "nombre":
                    juegos = juegos.OrderBy(j => j.Titulo);
                    break;
                case "anio":
                    juegos = juegos.OrderByDescending(j => j.AnioLanzamiento);
                    break;
                default:
                    juegos = juegos.OrderByDescending(j => j.Destacado).ThenBy(j => j.Titulo);
                    break;
            }

            var paginaActual = pagina ?? 1;
            var totalJuegos = juegos.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalJuegos / ITEMS_POR_PAGINA);

            var juegosPaginados = juegos
                .Skip((paginaActual - 1) * ITEMS_POR_PAGINA)
                .Take(ITEMS_POR_PAGINA)
                .ToList();

            ViewBag.Consolas = todasLasConsolas;
            ViewBag.Generos = todosLosJuegos.Select(j => j.Genero).Distinct().OrderBy(g => g).ToList();
            ViewBag.ConsolaId = consolaId;
            ViewBag.GeneroActual = genero;
            ViewBag.OrdenActual = orden;
            ViewBag.PaginaActual = paginaActual;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalJuegos = totalJuegos;

            return View(juegosPaginados);
        }

        public ActionResult PorConsola(int consolaId)
        {
            return RedirectToAction("Index", new { consolaId = consolaId });
        }

        public ActionResult Detalle(int id, string slug)
        {
            var juego = _catalogoService.ObtenerJuegoPorId(id);

            if (juego == null || !juego.Activo)
            {
                return RedirectToAction("Index");
            }

            var slugCorrecto = juego.ObtenerSlug();
            if (!string.IsNullOrEmpty(slug) && slug != slugCorrecto)
            {
                return RedirectToAction("Detalle", new { id = id, slug = slugCorrecto });
            }

            var consola = _catalogoService.ObtenerConsolaPorId(juego.ConsolaId);
            ViewBag.Consola = consola;

            var todosLosJuegos = _catalogoService.ObtenerJuegos();
            var juegosRelacionados = todosLosJuegos
                .Where(j => j.Id != juego.Id && j.Activo && 
                       (j.ConsolaId == juego.ConsolaId || j.Genero == juego.Genero))
                .Take(4)
                .ToList();
            ViewBag.JuegosRelacionados = juegosRelacionados;

            return View(juego);
        }

        [HttpPost]
        public ActionResult ObtenerStock(int juegoId)
        {
            var juego = _catalogoService.ObtenerJuegoPorId(juegoId);
            if (juego == null)
            {
                return Json(new { success = false, mensaje = "Juego no encontrado" });
            }

            return Json(new { success = true, stock = juego.Stock });
        }
    }
}
