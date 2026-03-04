using System;

namespace SoplaElCartucho.Web.Models
{
    /*
     * 🎮 SOPLA EL CARTUCHO - Modelo Juego
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Modelo anémico sin encapsulación
     * 2. Precio como decimal sin restricciones (podría ser negativo)
     * 3. Relación con Consola por ID sin navegación lazy loading
     * 4. Sin auditoría (CreatedAt, UpdatedAt)
     * 
     * 📝 MIGRACIÓN:
     * - Usar record para inmutabilidad
     * - Añadir DataAnnotations completas
     * - Implementar value objects para Precio
     * - Añadir campos de auditoría con EF Core
     */
    public class Juego
    {
        public int Id { get; set; }
        
        // ⚠️ LEGACY: Sin [Required] ni [MaxLength]
        public string Titulo { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Descripción HTML permitida (XSS potencial)
        public string Descripcion { get; set; }
        
        // ⚠️ LEGACY: Precio puede ser negativo
        public decimal Precio { get; set; }
        
        // ⚠️ LEGACY: Stock puede ser negativo (debería ser uint)
        public int Stock { get; set; }
        
        // ⚠️ ANTI-PATRÓN: URL de imagen sin validación
        public string ImagenUrl { get; set; }
        
        // ⚠️ LEGACY: Relación solo por FK, sin objeto de navegación
        public int ConsolaId { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Campo género como string libre
        public string Genero { get; set; }
        
        // ⚠️ LEGACY: Año de lanzamiento como int
        public int AnioLanzamiento { get; set; }
        
        // ⚠️ LEGACY: Desarrollador como string libre
        public string Desarrollador { get; set; }
        
        // ⚠️ ANTI-PATRÓN: Campo "estado" como string magic
        public string Estado { get; set; } // "Nuevo", "BuenEstado", "Usado"
        
        // ⚠️ LEGACY: Sin timestamps de auditoría
        public DateTime? FechaAlta { get; set; }
        
        public bool Destacado { get; set; }
        public bool Activo { get; set; }

        // ⚠️ ANTI-PATRÓN: Lógica de negocio en el modelo (debería estar en servicio)
        public decimal PrecioConIVA
        {
            get 
            { 
                // ⚠️ LEGACY: IVA hardcodeado
                return Precio * 1.21m; 
            }
        }

        // ⚠️ LEGACY: Método helper en modelo (rompe separación de responsabilidades)
        public string ObtenerSlug()
        {
            if (string.IsNullOrEmpty(Titulo)) return string.Empty;
            
            // ⚠️ ANTI-PATRÓN: Generación de slug básica sin normalización
            return Titulo.ToLower()
                .Replace(" ", "-")
                .Replace(":", "")
                .Replace("'", "");
        }
    }
}
