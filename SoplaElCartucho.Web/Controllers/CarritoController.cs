using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoplaElCartucho.Web.Models;

namespace SoplaElCartucho.Web.Controllers
{
    /*
     * 🎮 SOPLA EL CARTUCHO - CarritoController Legacy
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Uso intensivo de Session (no escala en web farms)
     * 2. Sin validación CSRF en algunas acciones
     * 3. Lógica de negocio en controlador
     * 4. Casting de Session sin null checks adecuados
     * 
     * 📝 MIGRACIÓN:
     * - Usar IDistributedCache (Redis) en lugar de Session
     * - Añadir [ValidateAntiForgeryToken] a todas las acciones POST
     * - Mover lógica a CarritoService
     * - Implementar carrito persistente en base de datos
     */
    public class CarritoController : Controller
    {
        // ⚠️ ANTI-PATRÓN: Acceso directo a datos (debería inyectar servicio)
        // Esto es una referencia temporal - en producción vendría de la BD
        private static List<Juego> _juegos = new List<Juego>
        {
            new Juego { Id = 1, Titulo = "Super Mario Bros 3", ConsolaId = 1, Precio = 29.99m, Stock = 5, ImagenUrl = "/Content/images/juegos/smb3.png" },
            new Juego { Id = 4, Titulo = "The Legend of Zelda: A Link to the Past", ConsolaId = 2, Precio = 44.99m, Stock = 4, ImagenUrl = "/Content/images/juegos/zelda-alttp.png" },
            new Juego { Id = 7, Titulo = "Sonic the Hedgehog 2", ConsolaId = 3, Precio = 24.99m, Stock = 8, ImagenUrl = "/Content/images/juegos/sonic2.png" },
            new Juego { Id = 9, Titulo = "Final Fantasy VII", ConsolaId = 4, Precio = 49.99m, Stock = 3, ImagenUrl = "/Content/images/juegos/ff7.png" }
        };

        // GET: /Carrito
        public ActionResult Index()
        {
            // ⚠️ ANTI-PATRÓN: Obtener carrito de Session con cast inseguro
            var carrito = ObtenerCarrito();
            
            // ⚠️ LEGACY: Cálculos en controlador
            decimal subtotal = 0;
            foreach (var item in carrito)
            {
                subtotal += item.Subtotal;
            }
            
            // ⚠️ ANTI-PATRÓN: IVA hardcodeado
            decimal iva = subtotal * 0.21m;
            decimal gastosEnvio = subtotal > 50 ? 0 : 4.99m;
            decimal total = subtotal + iva + gastosEnvio;

            ViewBag.Subtotal = subtotal;
            ViewBag.IVA = iva;
            ViewBag.GastosEnvio = gastosEnvio;
            ViewBag.Total = total;
            ViewBag.EnvioGratis = subtotal > 50;

            return View(carrito);
        }

        // POST: /Carrito/Agregar
        [HttpPost]
        // ⚠️ ANTI-PATRÓN: Sin [ValidateAntiForgeryToken] - vulnerable a CSRF
        public ActionResult Agregar(int juegoId, int cantidad = 1)
        {
            // ⚠️ LEGACY: Búsqueda lineal en lista estática
            var juego = _juegos.FirstOrDefault(j => j.Id == juegoId);
            
            if (juego == null)
            {
                // ⚠️ ANTI-PATRÓN: TempData para mensajes (funciona pero no es ideal)
                TempData["Error"] = "¡Ups! Ese juego no existe. ¿Seguro que no soñaste con él? 🎮";
                return RedirectToAction("Index", "Catalogo");
            }

            // ⚠️ LEGACY: Validación básica de stock
            if (juego.Stock < cantidad)
            {
                TempData["Error"] = $"¡Lo sentimos! Solo quedan {juego.Stock} unidades de {juego.Titulo}. ¡Son muy buscados! 🔥";
                return RedirectToAction("Detalle", "Catalogo", new { id = juegoId });
            }

            // ⚠️ ANTI-PATRÓN: Lógica de carrito en controlador
            var carrito = ObtenerCarrito();
            var itemExistente = carrito.FirstOrDefault(i => i.JuegoId == juegoId);

            if (itemExistente != null)
            {
                // ⚠️ LEGACY: Validación de cantidad máxima hardcodeada
                if (itemExistente.Cantidad + cantidad > 3)
                {
                    TempData["Error"] = "¡Máximo 3 unidades por juego! Dejamos algo para otros gamers 😉";
                    return RedirectToAction("Index");
                }
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // ⚠️ LEGACY: Máximo items en carrito hardcodeado
                if (carrito.Count >= 10)
                {
                    TempData["Error"] = "¡Tu carrito está lleno! Máximo 10 juegos diferentes. 🛒";
                    return RedirectToAction("Index");
                }

                carrito.Add(new CarritoItem(juego, cantidad));
            }

            // ⚠️ ANTI-PATRÓN: Guardar en Session y actualizar contador
            GuardarCarrito(carrito);
            TempData["Exito"] = $"¡{juego.Titulo} añadido al carrito! 🎉";

            return RedirectToAction("Index");
        }

        // POST: /Carrito/Actualizar
        [HttpPost]
        // ⚠️ ANTI-PATRÓN: Sin [ValidateAntiForgeryToken]
        public ActionResult Actualizar(int juegoId, int cantidad)
        {
            if (cantidad < 1)
            {
                return Eliminar(juegoId);
            }

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(i => i.JuegoId == juegoId);

            if (item != null)
            {
                // ⚠️ LEGACY: Validación de máximo inline
                item.Cantidad = Math.Min(cantidad, 3);
                GuardarCarrito(carrito);
            }

            // ⚠️ ANTI-PATRÓN: Mezcla de redirects y responses JSON
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, nuevoTotal = item?.Subtotal ?? 0 });
            }

            return RedirectToAction("Index");
        }

        // POST: /Carrito/Eliminar
        [HttpPost]
        public ActionResult Eliminar(int juegoId)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(i => i.JuegoId == juegoId);

            if (item != null)
            {
                carrito.Remove(item);
                GuardarCarrito(carrito);
                TempData["Exito"] = $"¡{item.Titulo} eliminado del carrito!";
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }

            return RedirectToAction("Index");
        }

        // POST: /Carrito/Vaciar
        [HttpPost]
        public ActionResult Vaciar()
        {
            // ⚠️ ANTI-PATRÓN: Limpiar session directamente
            Session["Carrito"] = new List<CarritoItem>();
            Session["CarritoCount"] = 0;
            
            TempData["Exito"] = "Carrito vaciado. ¡Empieza de nuevo la aventura! 🔄";
            return RedirectToAction("Index");
        }

        // GET: /Carrito/ObtenerCantidad (AJAX)
        public ActionResult ObtenerCantidad()
        {
            // ⚠️ LEGACY: Endpoint simple para actualizar badge del carrito
            var count = Session["CarritoCount"] ?? 0;
            return Json(new { cantidad = count }, JsonRequestBehavior.AllowGet);
        }

        #region Métodos Privados Legacy

        // ⚠️ ANTI-PATRÓN: Métodos helper en controlador (debería estar en servicio)
        private List<CarritoItem> ObtenerCarrito()
        {
            // ⚠️ LEGACY: Cast de Session con null check básico
            var carrito = Session["Carrito"] as List<CarritoItem>;
            if (carrito == null)
            {
                carrito = new List<CarritoItem>();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }

        private void GuardarCarrito(List<CarritoItem> carrito)
        {
            Session["Carrito"] = carrito;
            
            // ⚠️ LEGACY: Calcular cantidad total con bucle
            int total = 0;
            foreach (var item in carrito)
            {
                total += item.Cantidad;
            }
            Session["CarritoCount"] = total;
        }

        #endregion
    }
}
