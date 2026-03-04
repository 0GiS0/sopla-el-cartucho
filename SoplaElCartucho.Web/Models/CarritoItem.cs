using System;

namespace SoplaElCartucho.Web.Models
{
    /*
     * 🎮 SOPLA EL CARTUCHO - Modelo Item de Carrito
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Clase simple sin validación
     * 2. Se almacena en Session (no serializable correctamente)
     * 3. Sin control de cantidad máxima
     * 
     * 📝 MIGRACIÓN:
     * - Implementar como record
     * - Almacenar en cache distribuido (Redis)
     * - Validar cantidad contra stock
     */
    [Serializable] // ⚠️ LEGACY: Necesario para almacenar en Session
    public class CarritoItem
    {
        public int JuegoId { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Duplicamos datos del juego (denormalización)
        public string Titulo { get; set; }
        public string ImagenUrl { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string NombreConsola { get; set; }
        
        // ⚠️ LEGACY: Cantidad sin validación de mínimo/máximo
        public int Cantidad { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Fecha añadido sin zona horaria
        public DateTime FechaAgregado { get; set; }

        // ⚠️ LEGACY: Cálculo de subtotal en modelo
        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }

        // ⚠️ ANTI-PATRÓN: Constructor vacío requerido para serialización
        public CarritoItem() { }

        public CarritoItem(Juego juego, int cantidad)
        {
            JuegoId = juego.Id;
            Titulo = juego.Titulo;
            ImagenUrl = juego.ImagenUrl;
            PrecioUnitario = juego.Precio;
            Cantidad = cantidad;
            FechaAgregado = DateTime.Now; // ⚠️ LEGACY: DateTime.Now en lugar de UTC
        }
    }
}
