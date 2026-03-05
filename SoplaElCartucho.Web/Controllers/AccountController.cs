using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace SoplaElCartucho.Web.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe, string returnUrl)
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(rememberMe ? 30 : 1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

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
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword)
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            TempData["Exito"] = $"¡Bienvenido a Sopla el Cartucho, {username}! 🎉 Tu cuenta ha sido creada.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            HttpContext.Session.Clear();

            TempData["Exito"] = "¡Hasta pronto, gamer! 👋 Vuelve cuando quieras.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var username = User.Identity?.Name ?? "Gamer";
            
            ViewBag.Username = username;
            ViewBag.Email = $"{username.ToLower()}@example.com";
            ViewBag.FechaRegistro = DateTime.Now.AddMonths(-3);
            ViewBag.TotalPedidos = 5;

            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
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
