using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SoplaElCartucho.Business.Services;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Web.Controllers
{
    /// <summary>
    /// Controlador principal de la aplicación.
    /// ⚠️ ANTI-PATRÓN LEGACY: Instanciación directa del servicio sin DI.
    /// 📝 MIGRACIÓN: Usar inyección de dependencias en constructor.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public HomeController()
        {
            _catalogoService = new CatalogoService();
        }

        public ActionResult Index()
        {
            ViewBag.Titulo = "🎮 Sopla el Cartucho";
            ViewBag.Subtitulo = "Tu tienda de videojuegos retro favorita";

            // Obtener datos desde la base de datos
            var consolasDestacadas = _catalogoService.ObtenerConsolasActivas();
            var juegosDestacados = _catalogoService.ObtenerJuegosDestacados();

            ViewData["ConsolasDestacadas"] = consolasDestacadas;
            ViewData["JuegosDestacados"] = juegosDestacados;

            var hora = DateTime.Now.Hour;
            if (hora >= 6 && hora < 12)
                ViewBag.Saludo = "¡Buenos días, gamer! 🌅";
            else if (hora >= 12 && hora < 20)
                ViewBag.Saludo = "¡Buenas tardes, gamer! ☀️";
            else
                ViewBag.Saludo = "¡Buenas noches, gamer! 🌙 Es hora de jugar...";

            var carritoCount = Session["CarritoCount"] ?? 0;
            ViewBag.CarritoCount = carritoCount;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Historia = @"
                🎮 SOPLA EL CARTUCHO nació en 2023 de la nostalgia de dos amigos 
                que crecieron jugando a la NES y la Mega Drive.

                Nuestra misión: rescatar esos cartuchos polvorientos de los 
                armarios y darles una nueva vida.

                💨 ¿Por qué 'Sopla el Cartucho'? 
                Porque todos lo hemos hecho: soplar el cartucho para que funcione.
                Es el ritual universal de todo gamer retro.
            ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Email = "soplaelcartucho@example.com";
            ViewBag.Telefono = "+34 666 123 456";
            ViewBag.Direccion = "Calle Pixel 8, Bit City";
            ViewBag.Horario = "Lunes a Viernes: 10:00 - 20:00";

            return View();
        }

        [HttpGet]
        public ActionResult Search(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index");
            }

            // Buscar en todos los juegos de la base de datos
            var todosLosJuegos = _catalogoService.ObtenerJuegos();
            var resultados = new List<Juego>();

            foreach (var juego in todosLosJuegos)
            {
                if (juego.Titulo.ToLower().Contains(q.ToLower()))
                {
                    resultados.Add(juego);
                }
            }

            ViewBag.Query = q;
            ViewBag.Resultados = resultados;
            ViewBag.TotalResultados = resultados.Count;

            return View("SearchResults");
        }
    }
}
