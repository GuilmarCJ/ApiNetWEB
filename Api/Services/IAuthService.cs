using Api.Models;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);
        Task<object> DebugInfo();
    }
}
