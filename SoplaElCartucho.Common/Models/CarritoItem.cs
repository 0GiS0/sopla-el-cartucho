using System;

namespace SoplaElCartucho.Common.Models
{
    [Serializable]
    public class CarritoItem
    {
        public int JuegoId { get; set; }
        public string Titulo { get; set; }
        public string ImagenUrl { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string NombreConsola { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAgregado { get; set; }

        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }

        public CarritoItem() { }

        public CarritoItem(Juego juego, int cantidad)
        {
            JuegoId = juego.Id;
            Titulo = juego.Titulo;
            ImagenUrl = juego.ImagenUrl;
            PrecioUnitario = juego.Precio;
            Cantidad = cantidad;
            FechaAgregado = DateTime.Now;
        }
    }
}
