using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SoplaElCartucho.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            System.Diagnostics.Debug.WriteLine("🎮 ¡Sopla el Cartucho ha iniciado! 💨");
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.MapRoute(
                name: "CatalogoConsola",
                url: "Catalogo/Consola/{consolaId}",
                defaults: new { controller = "Catalogo", action = "PorConsola" }
            );

            routes.MapRoute(
                name: "DetalleJuego",
                url: "Juego/{id}/{slug}",
                defaults: new { controller = "Catalogo", action = "Detalle", slug = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            System.Diagnostics.Debug.WriteLine("❌ ERROR: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("   Stack: " + ex.StackTrace);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Carrito"] = new System.Collections.Generic.List<object>();
            Session["CarritoCount"] = 0;
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }
    }
}
