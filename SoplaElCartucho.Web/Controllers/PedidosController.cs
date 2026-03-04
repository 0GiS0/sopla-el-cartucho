using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SoplaElCartucho.Web.Models;

namespace SoplaElCartucho.Web.Controllers
{
    /*
     * 🎮 SOPLA EL CARTUCHO - PedidosController Legacy
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Autorización con atributo [Authorize] básico
     * 2. Acceso a datos con lista estática (debería usar DataSets/EF)
     * 3. Sin transacciones para operaciones críticas
     * 4. Generación de número de pedido no concurrency-safe
     * 
     * 📝 MIGRACIÓN:
     * - Usar [Authorize(Policy = "...")] con políticas
     * - Inyectar IPedidoService con Unit of Work
     * - Implementar transacciones con TransactionScope o EF Core
     * - Usar Guid o secuencias de BD para números de pedido
     */
    [Authorize] // ⚠️ LEGACY: Autorización básica sin roles específicos
    public class PedidosController : Controller
    {
        // ⚠️ ANTI-PATRÓN: Lista estática como "base de datos" temporal
        private static List<Pedido> _pedidos = new List<Pedido>();
        private static int _ultimoId = 0;

        // GET: /Pedidos
        public ActionResult Index()
        {
            // ⚠️ LEGACY: Obtener usuario de Membership
            var nombreUsuario = User.Identity.Name;
            
            // ⚠️ ANTI-PATRÓN: Filtrar en memoria (debería ser en BD)
            var misPedidos = _pedidos
                .Where(p => p.NombreUsuario == nombreUsuario)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return View(misPedidos);
        }

        // GET: /Pedidos/Detalle/1
        public ActionResult Detalle(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido == null)
            {
                TempData["Error"] = "Pedido no encontrado. ¿Seguro que existe? 🤔";
                return RedirectToAction("Index");
            }

            // ⚠️ LEGACY: Verificación de autorización manual
            if (pedido.NombreUsuario != User.Identity.Name && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "No puedes ver pedidos de otros usuarios. ¡Eso no está bien! 🚫";
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // GET: /Pedidos/Checkout
        public ActionResult Checkout()
        {
            // ⚠️ LEGACY: Obtener carrito de Session
            var carrito = Session["Carrito"] as List<CarritoItem>;
            
            if (carrito == null || carrito.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío. ¡Añade algunos juegos primero! 🎮";
                return RedirectToAction("Index", "Catalogo");
            }

            // ⚠️ ANTI-PATRÓN: Crear pedido parcialmente para la vista
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

        // POST: /Pedidos/ConfirmarPedido
        [HttpPost]
        [ValidateAntiForgeryToken] // ⚠️ Al menos aquí sí lo pusieron!
        public ActionResult ConfirmarPedido(string direccion, string ciudad, string codigoPostal, string telefono, string comentarios)
        {
            // ⚠️ LEGACY: Validación manual sin DataAnnotations
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

            // ⚠️ ANTI-PATRÓN: Generación de ID no thread-safe
            _ultimoId++;
            
            // ⚠️ LEGACY: Crear pedido con todos los datos
            var pedido = new Pedido
            {
                Id = _ultimoId,
                NombreUsuario = User.Identity.Name,
                Estado = "Pendiente",
                FechaPedido = DateTime.Now, // ⚠️ Sin UTC
                DireccionEnvio = direccion,
                CiudadEnvio = ciudad,
                CodigoPostalEnvio = codigoPostal,
                PaisEnvio = "España", // ⚠️ HARDCODED
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

            // ⚠️ ANTI-PATRÓN: Sin transacción - si falla después, el pedido queda inconsistente
            _pedidos.Add(pedido);

            // ⚠️ LEGACY: Vaciar carrito manualmente
            Session["Carrito"] = new List<CarritoItem>();
            Session["CarritoCount"] = 0;

            TempData["Exito"] = $"¡Pedido #{pedido.Id} confirmado! 🎉 Gracias por tu compra.";
            return RedirectToAction("Confirmacion", new { id = pedido.Id });
        }

        // GET: /Pedidos/Confirmacion/1
        public ActionResult Confirmacion(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido == null || pedido.NombreUsuario != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // POST: /Pedidos/Cancelar
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

            // ⚠️ LEGACY: Verificación de autorización manual
            if (pedido.NombreUsuario != User.Identity.Name)
            {
                TempData["Error"] = "No puedes cancelar pedidos de otros usuarios.";
                return RedirectToAction("Index");
            }

            // ⚠️ ANTI-PATRÓN: Verificar estado con string magic
            if (pedido.Estado != "Pendiente" && pedido.Estado != "Procesando")
            {
                TempData["Error"] = "Este pedido ya no se puede cancelar. 😢";
                return RedirectToAction("Detalle", new { id = id });
            }

            // ⚠️ LEGACY: Sin lógica de revertir stock
            pedido.CambiarEstado("Cancelado");
            
            TempData["Exito"] = "Pedido cancelado correctamente.";
            return RedirectToAction("Index");
        }

        #region Admin Actions

        // GET: /Pedidos/Admin (Solo para administradores)
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            // ⚠️ LEGACY: Sin paginación
            var todosPedidos = _pedidos.OrderByDescending(p => p.FechaPedido).ToList();
            return View(todosPedidos);
        }

        // POST: /Pedidos/CambiarEstado (Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            
            if (pedido != null)
            {
                // ⚠️ ANTI-PATRÓN: Sin validación de transiciones válidas
                pedido.CambiarEstado(nuevoEstado);
                TempData["Exito"] = $"Estado del pedido #{id} actualizado a '{nuevoEstado}'.";
            }

            return RedirectToAction("Admin");
        }

        #endregion
    }
}
