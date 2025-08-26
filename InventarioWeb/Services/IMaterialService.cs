using InventarioWeb.Models;

namespace InventarioWeb.Services
{
    public interface IMaterialService
    {
        Task<MaterialesResponse> GetMaterialesAsync(int page = 1, int pageSize = 50, string local = null);
        Task<List<string>> GetLocalesAsync();
    }
}
