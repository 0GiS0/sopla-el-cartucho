using System;

namespace SoplaElCartucho.Web.Models
{
    public class Consola
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public string Fabricante { get; set; }
        public int AnioLanzamiento { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public bool Activa { get; set; }
    }
}
