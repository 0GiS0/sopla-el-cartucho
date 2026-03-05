using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SoplaElCartucho.Web.Models;

namespace SoplaElCartucho.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Consola> _consolasDestacadas = new List<Consola>
        {
            new Consola { Id = 1, Nombre = "NES", Fabricante = "Nintendo", AnioLanzamiento = 1983, ImagenUrl = "/Content/images/consolas/nes.svg" },
            new Consola { Id = 2, Nombre = "SNES", Fabricante = "Nintendo", AnioLanzamiento = 1990, ImagenUrl = "/Content/images/consolas/snes.svg" },
            new Consola { Id = 3, Nombre = "Mega Drive", Fabricante = "SEGA", AnioLanzamiento = 1988, ImagenUrl = "/Content/images/consolas/megadrive.svg" },
            new Consola { Id = 4, Nombre = "PlayStation", Fabricante = "Sony", AnioLanzamiento = 1994, ImagenUrl = "/Content/images/consolas/ps1.svg" },
            new Consola { Id = 5, Nombre = "Nintendo 64", Fabricante = "Nintendo", AnioLanzamiento = 1996, ImagenUrl = "/Content/images/consolas/n64.svg" },
            new Consola { Id = 6, Nombre = "Game Boy", Fabricante = "Nintendo", AnioLanzamiento = 1989, ImagenUrl = "/Content/images/consolas/gameboy.svg" }
        };

        private static readonly List<Juego> _juegosDestacados = new List<Juego>
        {
            new Juego { Id = 1, Titulo = "Super Mario Bros 3", ConsolaId = 1, Precio = 29.99m, ImagenUrl = "/Content/images/juegos/smb3.png", Genero = "Plataformas", AnioLanzamiento = 1988, Desarrollador = "Nintendo", Destacado = true },
            new Juego { Id = 2, Titulo = "The Legend of Zelda: A Link to the Past", ConsolaId = 2, Precio = 39.99m, ImagenUrl = "/Content/images/juegos/zelda-alttp.jpg", Genero = "Aventura", AnioLanzamiento = 1991, Desarrollador = "Nintendo", Destacado = true },
            new Juego { Id = 3, Titulo = "Sonic the Hedgehog 2", ConsolaId = 3, Precio = 24.99m, ImagenUrl = "/Content/images/juegos/sonic2.jpg", Genero = "Plataformas", AnioLanzamiento = 1992, Desarrollador = "SEGA", Destacado = true },
            new Juego { Id = 4, Titulo = "Final Fantasy VII", ConsolaId = 4, Precio = 49.99m, ImagenUrl = "/Content/images/juegos/ff7.jpg", Genero = "RPG", AnioLanzamiento = 1997, Desarrollador = "Square", Destacado = true }
        };

        public ActionResult Index()
        {
            ViewBag.Titulo = "🎮 Sopla el Cartucho";
            ViewBag.Subtitulo = "Tu tienda de videojuegos retro favorita";
            ViewData["ConsolasDestacadas"] = _consolasDestacadas;
            ViewData["JuegosDestacados"] = _juegosDestacados;
            
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

            var resultados = new List<Juego>();
            foreach (var juego in _juegosDestacados)
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
