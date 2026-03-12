using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SoplaElCartucho.Business.Services;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Web.Controllers
{

    [Authorize]
    public class PedidosController : Controller
    {
        private readonly PedidoService _pedidoService;

        public PedidosController()
        {
            _pedidoService = new PedidoService();
        }

        public ActionResult Index()
        {
            var nombreUsuario = User.Identity.Name;

            var misPedidos = _pedidoService.ObtenerPedidosPorUsuario(nombreUsuario);

            return View(misPedidos);
        }

        public ActionResult Detalle(int id)
        {
            var pedido = _pedidoService.ObtenerPedidoPorId(id);

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

            var pedido = new Pedido
            {
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
                    JuegoId = item.JuegoId,
                    TituloJuego = item.Titulo,
                    ImagenJuego = item.ImagenUrl,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad
                }).ToList()
            };

            pedido.CalcularTotales();

            // Guardar en base de datos y obtener el ID generado
            int pedidoId = _pedidoService.CrearPedido(pedido);
            pedido.Id = pedidoId;

            Session["Carrito"] = new List<CarritoItem>();
            Session["CarritoCount"] = 0;

            TempData["Exito"] = $"¡Pedido #{pedido.Id} confirmado! 🎉 Gracias por tu compra.";
            return RedirectToAction("Confirmacion", new { id = pedido.Id });
        }

        public ActionResult Confirmacion(int id)
        {
            var pedido = _pedidoService.ObtenerPedidoPorId(id);

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
            var pedido = _pedidoService.ObtenerPedidoPorId(id);

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

            _pedidoService.ActualizarEstado(id, "Cancelado");

            TempData["Exito"] = "Pedido cancelado correctamente.";
            return RedirectToAction("Index");
        }

        #region Admin Actions

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var todosPedidos = _pedidoService.ObtenerPedidos();
            return View(todosPedidos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = _pedidoService.ObtenerPedidoPorId(id);

            if (pedido != null)
            {
                _pedidoService.ActualizarEstado(id, nuevoEstado);
                TempData["Exito"] = $"Estado del pedido #{id} actualizado a '{nuevoEstado}'.";
            }

            return RedirectToAction("Admin");
        }

        #endregion
    }
}
