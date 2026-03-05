using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SoplaElCartucho.Web.Models;

namespace SoplaElCartucho.Web.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private static List<Pedido> _pedidos = new List<Pedido>();
        private static int _ultimoId = 0;

        public ActionResult Index()
        {
            var nombreUsuario = User.Identity.Name;
            
            var misPedidos = _pedidos
                .Where(p => p.NombreUsuario == nombreUsuario)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return View(misPedidos);
        }

        public ActionResult Detalle(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido == null)
            {
                TempData["Error"] = "Pedido no encontrado. ¿Seguro que existe? 🤔";
                return RedirectToAction("Index");
            }

            if (pedido.NombreUsuario != User.Identity.Name && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "No puedes ver pedidos de otros usuarios. ¡Eso no está bien! 🚫";
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        public ActionResult Checkout()
        {
            var carrito = Session["Carrito"] as List<CarritoItem>;
            
            if (carrito == null || carrito.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío. ¡Añade algunos juegos primero! 🎮";
                return RedirectToAction("Index", "Catalogo");
            }

            var pedido = new Pedido
            {
                Detalles = carrito.Select(item => new DetallePedido
                {
                    JuegoId = item.JuegoId,
                    TituloJuego = item.Titulo,
                    ImagenJuego = item.ImagenUrl,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad
                }).ToList()
            };
            
            pedido.CalcularTotales();

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarPedido(string direccion, string ciudad, string codigoPostal, string telefono, string comentarios)
        {
            if (string.IsNullOrEmpty(direccion) || string.IsNullOrEmpty(ciudad) || string.IsNullOrEmpty(codigoPostal))
            {
                TempData["Error"] = "Por favor, completa todos los campos de envío. 📦";
                return RedirectToAction("Checkout");
            }

            var carrito = Session["Carrito"] as List<CarritoItem>;
            if (carrito == null || carrito.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío. ¡Vuelve a intentarlo! 🔄";
                return RedirectToAction("Index", "Catalogo");
            }

            _ultimoId++;
            
            var pedido = new Pedido
            {
                Id = _ultimoId,
                NombreUsuario = User.Identity.Name,
                Estado = "Pendiente",
                FechaPedido = DateTime.Now,
                DireccionEnvio = direccion,
                CiudadEnvio = ciudad,
                CodigoPostalEnvio = codigoPostal,
                PaisEnvio = "España",
                TelefonoContacto = telefono,
                Comentarios = comentarios,
                Detalles = carrito.Select(item => new DetallePedido
                {
                    PedidoId = _ultimoId,
                    JuegoId = item.JuegoId,
                    TituloJuego = item.Titulo,
                    ImagenJuego = item.ImagenUrl,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad
                }).ToList()
            };

            pedido.CalcularTotales();

            _pedidos.Add(pedido);

            Session["Carrito"] = new List<CarritoItem>();
            Session["CarritoCount"] = 0;

            TempData["Exito"] = $"¡Pedido #{pedido.Id} confirmado! 🎉 Gracias por tu compra.";
            return RedirectToAction("Confirmacion", new { id = pedido.Id });
        }

        public ActionResult Confirmacion(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido == null || pedido.NombreUsuario != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido == null)
            {
                TempData["Error"] = "Pedido no encontrado.";
                return RedirectToAction("Index");
            }

            if (pedido.NombreUsuario != User.Identity.Name)
            {
                TempData["Error"] = "No puedes cancelar pedidos de otros usuarios.";
                return RedirectToAction("Index");
            }

            if (pedido.Estado != "Pendiente" && pedido.Estado != "Procesando")
            {
                TempData["Error"] = "Este pedido ya no se puede cancelar. 😢";
                return RedirectToAction("Detalle", new { id = id });
            }

            pedido.CambiarEstado("Cancelado");
            
            TempData["Exito"] = "Pedido cancelado correctamente.";
            return RedirectToAction("Index");
        }

        #region Admin Actions

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var todosPedidos = _pedidos.OrderByDescending(p => p.FechaPedido).ToList();
            return View(todosPedidos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido != null)
            {
                pedido.CambiarEstado(nuevoEstado);
                TempData["Exito"] = $"Estado del pedido #{id} actualizado a '{nuevoEstado}'.";
            }

            return RedirectToAction("Admin");
        }

        #endregion
    }
}
