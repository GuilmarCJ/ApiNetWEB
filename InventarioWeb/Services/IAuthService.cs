using InventarioWeb.Models;

namespace InventarioWeb.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginViewModel login);
        Task<bool> IsTokenValid();
        void Logout();
    }
}
