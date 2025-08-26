using System.Security.Claims;
using System.Text;
using System.Text.Json;
using InventarioWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InventarioWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> LoginAsync(LoginViewModel login)
        {
            var client = _httpClientFactory.CreateClient("InventarioAPI");

            var json = JsonSerializer.Serialize(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Guardar token en sesión
                var context = _httpContextAccessor.HttpContext;
                context.Session.SetString("JwtToken", loginResponse.Token);
                context.Session.SetString("SessionId", loginResponse.SessionId);

                // Crear claims identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.Usuario.NombreUsuario),
                    new Claim(ClaimTypes.Role, loginResponse.Usuario.Rol),
                    new Claim("FullName", loginResponse.Usuario.NombreCompleto),
                    new Claim("Local", loginResponse.Usuario.LocalAsignado),
                    new Claim("SessionId", loginResponse.SessionId)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                };

                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return loginResponse;
            }

            return null;
        }

        public async Task<bool> IsTokenValid()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            return !string.IsNullOrEmpty(token);
        }

        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            context.Session.Remove("JwtToken");
            context.Session.Remove("SessionId");
            context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
