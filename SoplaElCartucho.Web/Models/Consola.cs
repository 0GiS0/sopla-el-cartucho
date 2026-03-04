using System;

namespace SoplaElCartucho.Web.Models
{
    /*
     * 🎮 SOPLA EL CARTUCHO - Modelo Consola
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Modelo anémico sin comportamiento (solo propiedades)
     * 2. Sin validación de datos (usar DataAnnotations o FluentValidation)
     * 3. Propiedades públicas con setters (expone estado interno)
     * 
     * 📝 MIGRACIÓN:
     * - Añadir [Required], [MaxLength] y otras DataAnnotations
     * - Considerar usar records en C# 9+
     * - Implementar validación con FluentValidation
     */
    public class Consola
    {
        public int Id { get; set; }
        
        // ⚠️ LEGACY: Sin validación - podría ser null o vacío
        public string Nombre { get; set; }
        
        // ⚠️ LEGACY: Sin validación de formato de imagen
        public string ImagenUrl { get; set; }
        
        public string Fabricante { get; set; }
        
        // ⚠️ LEGACY: Año como int, no como DateTime
        public int AnioLanzamiento { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Descripción puede ser muy larga sin límite
        public string Descripcion { get; set; }
        
        // ⚠️ LEGACY: Campo para ordenamiento manual
        public int Orden { get; set; }
        
        public bool Activa { get; set; }
    }
}
