using System;
using System.Web.Mvc;
using System.Web.Security;

namespace SoplaElCartucho.Web.Controllers
{
    /*
     * 🎮 SOPLA EL CARTUCHO - AccountController Legacy
     * 
     * ⚠️ ANTI-PATRONES EN ESTE ARCHIVO:
     * 1. Uso de Membership Provider (obsoleto desde .NET 4.5)
     * 2. FormsAuthentication (migrar a ASP.NET Core Identity + JWT/Cookies)
     * 3. Sin 2FA, sin OAuth, sin recuperación de contraseña moderna
     * 4. Almacenamiento de contraseñas con hash MD5/SHA1 (inseguro)
     * 
     * 📝 MIGRACIÓN:
     * - Usar ASP.NET Core Identity
     * - Implementar OAuth 2.0 (Google, GitHub, etc.)
     * - Añadir 2FA con TOTP
     * - Usar bcrypt o Argon2 para contraseñas
     */
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // ⚠️ LEGACY: Verificar si ya está autenticado
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, bool rememberMe, string returnUrl)
        {
            // ⚠️ LEGACY: Validación básica sin Model Binding
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, introduce usuario y contraseña. 🎮";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // ⚠️ ANTI-PATRÓN: Membership.ValidateUser usa algoritmos de hash obsoletos
            // ⚠️ DEMO: Para demostración, aceptamos cualquier usuario con password "retro123"
            bool esValido = ValidarUsuarioDemo(username, password);

            if (esValido)
            {
                // ⚠️ LEGACY: FormsAuthentication - obsoleto
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                
                // ⚠️ ANTI-PATRÓN: Sin validar returnUrl (Open Redirect vulnerability)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                TempData["Exito"] = $"¡Bienvenido de vuelta, {username}! 🎮 ¡A jugar!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos. ¿Has intentado soplar el cartucho? 💨";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {
            // ⚠️ LEGACY: Validación manual sin DataAnnotations
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Todos los campos son obligatorios. 📝";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden. ¡Inténtalo de nuevo! 🔐";
                return View();
            }

            // ⚠️ LEGACY: Validación de contraseña básica
            if (password.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres. 🔑";
                return View();
            }

            // ⚠️ ANTI-PATRÓN: Validación de email básica
            if (!email.Contains("@"))
            {
                ViewBag.Error = "Por favor, introduce un email válido. 📧";
                return View();
            }

            // ⚠️ DEMO: Simular registro exitoso
            // En producción usaríamos: Membership.CreateUser(username, password, email, ...)
            
            // ⚠️ LEGACY: Auto-login después del registro
            FormsAuthentication.SetAuthCookie(username, false);
            
            TempData["Exito"] = $"¡Bienvenido a Sopla el Cartucho, {username}! 🎉 Tu cuenta ha sido creada.";
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // ⚠️ LEGACY: FormsAuthentication.SignOut
            FormsAuthentication.SignOut();
            
            // ⚠️ ANTI-PATRÓN: Limpiar session manualmente
            Session.Clear();
            Session.Abandon();

            TempData["Exito"] = "¡Hasta pronto, gamer! 👋 Vuelve cuando quieras.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        [Authorize]
        public ActionResult Profile()
        {
            // ⚠️ LEGACY: Obtener usuario de Membership
            var username = User.Identity.Name;
            
            // ⚠️ ANTI-PATRÓN: Datos de usuario hardcodeados para demo
            ViewBag.Username = username;
            ViewBag.Email = $"{username.ToLower()}@example.com";
            ViewBag.FechaRegistro = DateTime.Now.AddMonths(-3); // Simulado
            ViewBag.TotalPedidos = 5; // Simulado

            return View();
        }

        // GET: /Account/AccessDenied
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        #region Helper Methods

        // ⚠️ DEMO: Método de validación para demostración
        private bool ValidarUsuarioDemo(string username, string password)
        {
            // ⚠️ INSEGURO: Solo para demostración - NUNCA hacer esto en producción
            // Cualquier usuario con contraseña "retro123" o "admin" puede entrar
            
            if (password == "retro123" || password == "admin")
            {
                return true;
            }

            // ⚠️ LEGACY: En producción usaríamos Membership.ValidateUser
            // return Membership.ValidateUser(username, password);
            
            return false;
        }

        #endregion
    }
}
