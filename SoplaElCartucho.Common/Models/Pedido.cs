using System;
using System.Collections.Generic;

namespace SoplaElCartucho.Common.Models
{
    [Serializable]
    public class Pedido
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string DireccionEnvio { get; set; }
        public string CiudadEnvio { get; set; }
        public string CodigoPostalEnvio { get; set; }
        public string PaisEnvio { get; set; }
        public string TelefonoContacto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal GastosEnvio { get; set; }
        public decimal Total { get; set; }
        public string Comentarios { get; set; }
        public List<DetallePedido> Detalles { get; set; }

        public void CalcularTotales()
        {
            if (Detalles == null) return;
            
            Subtotal = 0;
            foreach (var detalle in Detalles)
            {
                Subtotal += detalle.PrecioUnitario * detalle.Cantidad;
            }
            
            IVA = Subtotal * 0.21m;
            GastosEnvio = Subtotal > 50 ? 0 : 4.99m;
            Total = Subtotal + IVA + GastosEnvio;
        }

        public void CambiarEstado(string nuevoEstado)
        {
            Estado = nuevoEstado;
            
            if (nuevoEstado == "Enviado")
                FechaEnvio = DateTime.Now;
            else if (nuevoEstado == "Entregado")
                FechaEntrega = DateTime.Now;
        }
    }

    [Serializable]
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int JuegoId { get; set; }
        public string TituloJuego { get; set; }
        public string ImagenJuego { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        
        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }
    }
}
