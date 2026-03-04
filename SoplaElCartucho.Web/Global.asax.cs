using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SoplaElCartucho.Web
{
    /*
     * 🎮 SOPLA EL CARTUCHO - Global.asax Legacy
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Global.asax es un punto de acoplamiento global (migrar a Program.cs + Startup.cs)
     * 2. Registro de rutas hardcodeado (usar atributos de ruta o convenciones)
     * 3. Sin inyección de dependencias (usar IServiceCollection en .NET Core)
     * 4. Application_Start monolítico (dividir en servicios/middleware)
     * 
     * 📝 MIGRACIÓN A .NET CORE:
     * - Eliminar Global.asax completamente
     * - Configurar servicios en Program.cs con builder.Services
     * - Configurar middleware pipeline con app.UseXxx()
     * - Usar MapControllerRoute() para rutas
     */
    public class MvcApplication : HttpApplication
    {
        // ⚠️ LEGACY: Este método se ejecuta una vez al iniciar la aplicación
        protected void Application_Start()
        {
            // ⚠️ ANTI-PATRÓN: Registro global de áreas
            AreaRegistration.RegisterAllAreas();

            // ⚠️ ANTI-PATRÓN: Registro de rutas en código
            RegisterRoutes(RouteTable.Routes);

            // ⚠️ LEGACY: Mensaje de inicio en el log
            System.Diagnostics.Debug.WriteLine("🎮 ¡Sopla el Cartucho ha iniciado! 💨");
            System.Diagnostics.Debug.WriteLine("   Versión: 1.0.0 (Legacy .NET Framework 3.5)");
        }

        // ⚠️ LEGACY: Definición de rutas estática
        // 📝 MIGRACIÓN: Usar atributos [Route] o app.MapControllerRoute()
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            // Ruta para el catálogo con filtro de consola
            routes.MapRoute(
                name: "CatalogoConsola",
                url: "Catalogo/Consola/{consolaId}",
                defaults: new { controller = "Catalogo", action = "PorConsola" }
            );

            // Ruta para detalle de juego
            routes.MapRoute(
                name: "DetalleJuego",
                url: "Juego/{id}/{slug}",
                defaults: new { controller = "Catalogo", action = "Detalle", slug = UrlParameter.Optional }
            );

            // Ruta por defecto
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        // ⚠️ ANTI-PATRÓN: Manejo de errores global sin estructura
        // 📝 MIGRACIÓN: Usar middleware app.UseExceptionHandler()
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            
            // ⚠️ LEGACY: Log básico sin framework de logging
            System.Diagnostics.Debug.WriteLine("❌ ERROR: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("   Stack: " + ex.StackTrace);
        }

        // ⚠️ ANTI-PATRÓN: Lógica en eventos de sesión
        protected void Session_Start(object sender, EventArgs e)
        {
            // ⚠️ LEGACY: Inicializar carrito en sesión
            Session["Carrito"] = new System.Collections.Generic.List<object>();
            Session["CarritoCount"] = 0;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // ⚠️ LEGACY: Limpieza manual de sesión
            // En .NET Core esto se maneja automáticamente
        }
    }
}
