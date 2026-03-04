using System;
using System.Collections.Generic;

namespace SoplaElCartucho.Web.Models
{
    /*
     * 🎮 SOPLA EL CARTUCHO - Modelo Pedido
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Estado como string mágico (usar enum)
     * 2. Sin validación de transiciones de estado
     * 3. Datos de envío mezclados con pedido (violan SRP)
     * 4. Sin soft delete ni auditoría
     * 
     * 📝 MIGRACIÓN:
     * - Separar en Pedido + DireccionEnvio
     * - Usar enum para Estado con state machine
     * - Implementar eventos de dominio
     * - Añadir campos de auditoría
     */
    [Serializable]
    public class Pedido
    {
        public int Id { get; set; }
        
        // ⚠️ LEGACY: Relación con usuario por nombre, no por ID
        public string NombreUsuario { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Estado como string libre
        // Valores posibles: "Pendiente", "Procesando", "Enviado", "Entregado", "Cancelado"
        public string Estado { get; set; }
        
        // ⚠️ LEGACY: Fechas sin timezone
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Datos de envío en la misma entidad
        public string DireccionEnvio { get; set; }
        public string CiudadEnvio { get; set; }
        public string CodigoPostalEnvio { get; set; }
        public string PaisEnvio { get; set; }
        public string TelefonoContacto { get; set; }
        
        // ⚠️ LEGACY: Totales calculados y almacenados (denormalización)
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal GastosEnvio { get; set; }
        public decimal Total { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Comentarios como string largo sin estructura
        public string Comentarios { get; set; }
        
        // ⚠️ LEGACY: Lista de items sin inicializar (puede ser null)
        public List<DetallePedido> Detalles { get; set; }

        // ⚠️ ANTI-PATRÓN: Lógica de negocio en modelo
        public void CalcularTotales()
        {
            if (Detalles == null) return;
            
            Subtotal = 0;
            foreach (var detalle in Detalles)
            {
                Subtotal += detalle.PrecioUnitario * detalle.Cantidad;
            }
            
            // ⚠️ LEGACY: IVA hardcodeado al 21%
            IVA = Subtotal * 0.21m;
            
            // ⚠️ LEGACY: Gastos de envío hardcodeados
            GastosEnvio = Subtotal > 50 ? 0 : 4.99m;
            
            Total = Subtotal + IVA + GastosEnvio;
        }

        // ⚠️ ANTI-PATRÓN: Método de cambio de estado sin validación
        public void CambiarEstado(string nuevoEstado)
        {
            // ⚠️ LEGACY: Sin validación de transiciones válidas
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
        
        // ⚠️ ANTI-PATRÓN: Datos duplicados del juego al momento de compra
        public string TituloJuego { get; set; }
        public string ImagenJuego { get; set; }
        public decimal PrecioUnitario { get; set; }
        
        public int Cantidad { get; set; }
        
        // ⚠️ LEGACY: Cálculo en modelo
        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }
    }
}
