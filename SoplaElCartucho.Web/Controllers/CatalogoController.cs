using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SoplaElCartucho.Web.Models;

namespace SoplaElCartucho.Web.Controllers
{
    public class CatalogoController : Controller
    {
        private static List<Consola> _consolas = new List<Consola>
        {
            new Consola { Id = 1, Nombre = "NES", Fabricante = "Nintendo", AnioLanzamiento = 1983, ImagenUrl = "/Content/images/consolas/nes.svg", Activa = true, Orden = 1 },
            new Consola { Id = 2, Nombre = "SNES", Fabricante = "Nintendo", AnioLanzamiento = 1990, ImagenUrl = "/Content/images/consolas/snes.svg", Activa = true, Orden = 2 },
            new Consola { Id = 3, Nombre = "Mega Drive", Fabricante = "SEGA", AnioLanzamiento = 1988, ImagenUrl = "/Content/images/consolas/megadrive.svg", Activa = true, Orden = 3 },
            new Consola { Id = 4, Nombre = "PlayStation", Fabricante = "Sony", AnioLanzamiento = 1994, ImagenUrl = "/Content/images/consolas/ps1.svg", Activa = true, Orden = 4 },
            new Consola { Id = 5, Nombre = "Nintendo 64", Fabricante = "Nintendo", AnioLanzamiento = 1996, ImagenUrl = "/Content/images/consolas/n64.svg", Activa = true, Orden = 5 },
            new Consola { Id = 6, Nombre = "Game Boy", Fabricante = "Nintendo", AnioLanzamiento = 1989, ImagenUrl = "/Content/images/consolas/gameboy.svg", Activa = true, Orden = 6 }
        };

        private static List<Juego> _juegos = new List<Juego>
        {
            // NES
            new Juego { Id = 1, Titulo = "Super Mario Bros 3", ConsolaId = 1, Precio = 29.99m, Stock = 5, ImagenUrl = "/Content/images/juegos/smb3.png", Genero = "Plataformas", AnioLanzamiento = 1988, Desarrollador = "Nintendo", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 2, Titulo = "Mega Man 2", ConsolaId = 1, Precio = 34.99m, Stock = 3, ImagenUrl = "/Content/images/juegos/megaman2.jpg", Genero = "Acción", AnioLanzamiento = 1988, Desarrollador = "Capcom", Estado = "BuenEstado", Activo = true },
            new Juego { Id = 3, Titulo = "Castlevania", ConsolaId = 1, Precio = 39.99m, Stock = 2, ImagenUrl = "/Content/images/juegos/castlevania.jpg", Genero = "Acción", AnioLanzamiento = 1986, Desarrollador = "Konami", Estado = "Usado", Activo = true },
            
            // SNES  
            new Juego { Id = 4, Titulo = "The Legend of Zelda: A Link to the Past", ConsolaId = 2, Precio = 44.99m, Stock = 4, ImagenUrl = "/Content/images/juegos/zelda-alttp.jpg", Genero = "Aventura", AnioLanzamiento = 1991, Desarrollador = "Nintendo", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 5, Titulo = "Super Metroid", ConsolaId = 2, Precio = 54.99m, Stock = 2, ImagenUrl = "/Content/images/juegos/supermetroid.jpg", Genero = "Acción", AnioLanzamiento = 1994, Desarrollador = "Nintendo", Estado = "Nuevo", Activo = true },
            new Juego { Id = 6, Titulo = "Chrono Trigger", ConsolaId = 2, Precio = 89.99m, Stock = 1, ImagenUrl = "/Content/images/juegos/chronotrigger.jpg", Genero = "RPG", AnioLanzamiento = 1995, Desarrollador = "Square", Estado = "BuenEstado", Activo = true, Destacado = true },
            
            // Mega Drive
            new Juego { Id = 7, Titulo = "Sonic the Hedgehog 2", ConsolaId = 3, Precio = 24.99m, Stock = 8, ImagenUrl = "/Content/images/juegos/sonic2.jpg", Genero = "Plataformas", AnioLanzamiento = 1992, Desarrollador = "SEGA", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 8, Titulo = "Streets of Rage 2", ConsolaId = 3, Precio = 29.99m, Stock = 4, ImagenUrl = "/Content/images/juegos/sor2.jpg", Genero = "Beat'em up", AnioLanzamiento = 1992, Desarrollador = "SEGA", Estado = "Usado", Activo = true },
            
            // PlayStation
            new Juego { Id = 9, Titulo = "Final Fantasy VII", ConsolaId = 4, Precio = 49.99m, Stock = 3, ImagenUrl = "/Content/images/juegos/ff7.jpg", Genero = "RPG", AnioLanzamiento = 1997, Desarrollador = "Square", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 10, Titulo = "Metal Gear Solid", ConsolaId = 4, Precio = 39.99m, Stock = 5, ImagenUrl = "/Content/images/juegos/mgs.jpg", Genero = "Acción", AnioLanzamiento = 1998, Desarrollador = "Konami", Estado = "BuenEstado", Activo = true },
            new Juego { Id = 11, Titulo = "Resident Evil 2", ConsolaId = 4, Precio = 44.99m, Stock = 2, ImagenUrl = "/Content/images/juegos/re2.jpg", Genero = "Survival Horror", AnioLanzamiento = 1998, Desarrollador = "Capcom", Estado = "Usado", Activo = true },
            
            // Nintendo 64
            new Juego { Id = 12, Titulo = "GoldenEye 007", ConsolaId = 5, Precio = 34.99m, Stock = 6, ImagenUrl = "/Content/images/juegos/goldeneye.jpg", Genero = "FPS", AnioLanzamiento = 1997, Desarrollador = "Rare", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 13, Titulo = "Super Mario 64", ConsolaId = 5, Precio = 39.99m, Stock = 4, ImagenUrl = "/Content/images/juegos/sm64.jpg", Genero = "Plataformas", AnioLanzamiento = 1996, Desarrollador = "Nintendo", Estado = "BuenEstado", Activo = true },
            new Juego { Id = 14, Titulo = "The Legend of Zelda: Ocarina of Time", ConsolaId = 5, Precio = 59.99m, Stock = 2, ImagenUrl = "/Content/images/juegos/zelda-oot.jpg", Genero = "Aventura", AnioLanzamiento = 1998, Desarrollador = "Nintendo", Estado = "Nuevo", Activo = true, Destacado = true },
            
            // Game Boy
            new Juego { Id = 15, Titulo = "Pokémon Red", ConsolaId = 6, Precio = 29.99m, Stock = 7, ImagenUrl = "/Content/images/juegos/pokemon-red.jpg", Genero = "RPG", AnioLanzamiento = 1996, Desarrollador = "Game Freak", Estado = "BuenEstado", Activo = true, Destacado = true },
            new Juego { Id = 16, Titulo = "Tetris", ConsolaId = 6, Precio = 14.99m, Stock = 10, ImagenUrl = "/Content/images/juegos/tetris-gb.jpg", Genero = "Puzzle", AnioLanzamiento = 1989, Desarrollador = "Nintendo", Estado = "Usado", Activo = true }
        };

        private const int ITEMS_POR_PAGINA = 8;

        public ActionResult Index(int? consolaId, string genero, string orden, int? pagina)
        {
            var juegos = _juegos.Where(j => j.Activo);

            if (consolaId.HasValue)
            {
                juegos = juegos.Where(j => j.ConsolaId == consolaId.Value);
                ViewBag.ConsolaActual = _consolas.FirstOrDefault(c => c.Id == consolaId.Value);
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

            ViewBag.Consolas = _consolas.Where(c => c.Activa).OrderBy(c => c.Orden).ToList();
            ViewBag.Generos = _juegos.Select(j => j.Genero).Distinct().OrderBy(g => g).ToList();
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
            var juego = _juegos.FirstOrDefault(j => j.Id == id && j.Activo);
            
            if (juego == null)
            {
                return RedirectToAction("Index");
            }

            var slugCorrecto = juego.ObtenerSlug();
            if (!string.IsNullOrEmpty(slug) && slug != slugCorrecto)
            {
                return RedirectToAction("Detalle", new { id = id, slug = slugCorrecto });
            }

            var consola = _consolas.FirstOrDefault(c => c.Id == juego.ConsolaId);
            ViewBag.Consola = consola;

            var juegosRelacionados = _juegos
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
            var juego = _juegos.FirstOrDefault(j => j.Id == juegoId);
            if (juego == null)
            {
                return Json(new { success = false, mensaje = "Juego no encontrado" });
            }

            return Json(new { success = true, stock = juego.Stock });
        }
    }
}
