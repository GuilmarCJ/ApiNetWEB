using InventarioWeb.Models;
using InventarioWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            var sessionId = HttpContext.Session.GetString("SessionId");

            // Solo redirigir a Home si hay cookie Y token válido en sesión
            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", "Home");
            }

            // Si no, mostrar el login
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (result != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
