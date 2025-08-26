using System.Net.Http.Headers;
using System.Text.Json;
using InventarioWeb.Models;

namespace InventarioWeb.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<MaterialService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<MaterialesResponse> GetMaterialesAsync(int page = 1, int pageSize = 50, string local = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("InventarioAPI");

                var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
                var sessionId = _httpContextAccessor.HttpContext?.Session.GetString("SessionId");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionId))
                {
                    _logger.LogWarning("Token o SessionId no encontrados en la sesión");
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Remove("sessionId"); // evitar duplicados
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var url = $"api/Materiales?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(local))
                {
                    url += $"&local={local}";
                }

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MaterialesResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                _logger.LogWarning("Error al obtener materiales: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetMaterialesAsync");
                return null;
            }
        }

        public async Task<List<string>> GetLocalesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("InventarioAPI");

                var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
                var sessionId = _httpContextAccessor.HttpContext?.Session.GetString("SessionId");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionId))
                {
                    return new List<string>();
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Remove("sessionId");
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var response = await client.GetAsync("api/Materiales/locales");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result?["locales"] ?? new List<string>();
                }

                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetLocalesAsync");
                return new List<string>();
            }
        }
    }

}
