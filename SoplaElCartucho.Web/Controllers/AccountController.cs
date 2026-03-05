using System;
using System.Web.Mvc;
using System.Web.Security;

namespace SoplaElCartucho.Web.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, bool rememberMe, string returnUrl)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, introduce usuario y contraseña. 🎮";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            bool esValido = ValidarUsuario(username, password);

            if (esValido)
            {
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                
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

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {
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

            if (password.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres. 🔑";
                return View();
            }

            if (!email.Contains("@"))
            {
                ViewBag.Error = "Por favor, introduce un email válido. 📧";
                return View();
            }

            FormsAuthentication.SetAuthCookie(username, false);
            
            TempData["Exito"] = $"¡Bienvenido a Sopla el Cartucho, {username}! 🎉 Tu cuenta ha sido creada.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            
            Session.Clear();
            Session.Abandon();

            TempData["Exito"] = "¡Hasta pronto, gamer! 👋 Vuelve cuando quieras.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Profile()
        {
            var username = User.Identity.Name;
            
            ViewBag.Username = username;
            ViewBag.Email = $"{username.ToLower()}@example.com";
            ViewBag.FechaRegistro = DateTime.Now.AddMonths(-3);
            ViewBag.TotalPedidos = 5;

            return View();
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        #region Helper Methods

        private bool ValidarUsuario(string username, string password)
        {
            if (password == "retro123" || password == "admin")
            {
                return true;
            }
            
            return false;
        }

        #endregion
    }
}
