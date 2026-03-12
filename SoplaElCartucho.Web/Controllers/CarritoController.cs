using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoplaElCartucho.Business.Services;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Web.Controllers
{
    /// <summary>
    /// Controlador para gestión del carrito de compras.
    /// ⚠️ ANTI-PATRÓN LEGACY: Instanciación directa del servicio sin DI.
    /// 📝 MIGRACIÓN: Usar inyección de dependencias en constructor.
    /// Nota: El carrito se mantiene en Session (patrón válido para este escenario).
    /// </summary>
    public class CarritoController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public CarritoController()
        {
            _catalogoService = new CatalogoService();
        }

        public ActionResult Index()
        {
            var carrito = ObtenerCarrito();

            decimal subtotal = 0;
            foreach (var item in carrito)
            {
                subtotal += item.Subtotal;
            }

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

        [HttpPost]
        public ActionResult Agregar(int juegoId, int cantidad = 1)
        {
            // Obtener juego desde la base de datos
            var juego = _catalogoService.ObtenerJuegoPorId(juegoId);

            if (juego == null)
            {
                TempData["Error"] = "¡Ups! Ese juego no existe. ¿Seguro que no soñaste con él? 🎮";
                return RedirectToAction("Index", "Catalogo");
            }

            if (juego.Stock < cantidad)
            {
                TempData["Error"] = $"¡Lo sentimos! Solo quedan {juego.Stock} unidades de {juego.Titulo}. ¡Son muy buscados! 🔥";
                return RedirectToAction("Detalle", "Catalogo", new { id = juegoId });
            }

            var carrito = ObtenerCarrito();
            var itemExistente = carrito.FirstOrDefault(i => i.JuegoId == juegoId);

            if (itemExistente != null)
            {
                if (itemExistente.Cantidad + cantidad > 3)
                {
                    TempData["Error"] = "¡Máximo 3 unidades por juego! Dejamos algo para otros gamers 😉";
                    return RedirectToAction("Index");
                }
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                if (carrito.Count >= 10)
                {
                    TempData["Error"] = "¡Tu carrito está lleno! Máximo 10 juegos diferentes. 🛒";
                    return RedirectToAction("Index");
                }

                carrito.Add(new CarritoItem(juego, cantidad));
            }

            GuardarCarrito(carrito);
            TempData["Exito"] = $"¡{juego.Titulo} añadido al carrito! 🎉";

            return RedirectToAction("Index");
        }

        [HttpPost]
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
                item.Cantidad = Math.Min(cantidad, 3);
                GuardarCarrito(carrito);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, nuevoTotal = item?.Subtotal ?? 0 });
            }

            return RedirectToAction("Index");
        }

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

        [HttpPost]
        public ActionResult Vaciar()
        {
            Session["Carrito"] = new List<CarritoItem>();
            Session["CarritoCount"] = 0;

            TempData["Exito"] = "Carrito vaciado. ¡Empieza de nuevo la aventura! 🔄";
            return RedirectToAction("Index");
        }

        public ActionResult ObtenerCantidad()
        {
            var count = Session["CarritoCount"] ?? 0;
            return Json(new { cantidad = count }, JsonRequestBehavior.AllowGet);
        }

        #region Métodos Privados

        private List<CarritoItem> ObtenerCarrito()
        {
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
