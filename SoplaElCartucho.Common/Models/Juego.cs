using System;

namespace SoplaElCartucho.Common.Models
{
    public class Juego
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; }
        public int ConsolaId { get; set; }
        public string Genero { get; set; }
        public int AnioLanzamiento { get; set; }
        public string Desarrollador { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool Destacado { get; set; }
        public bool Activo { get; set; }

        public decimal PrecioConIVA
        {
            get 
            { 
                return Precio * 1.21m; 
            }
        }

        public string ObtenerSlug()
        {
            if (string.IsNullOrEmpty(Titulo)) return string.Empty;
            
            return Titulo.ToLower()
                .Replace(" ", "-")
                .Replace(":", "")
                .Replace("'", "");
        }
    }
}
