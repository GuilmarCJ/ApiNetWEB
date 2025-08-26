using Api.Models;

namespace Api.Services
{
    public interface IMaterialService
    {
        Task<MaterialResponse> GetMateriales(string userRole, string localAsignado, string nombreUsuario, MaterialRequest request);
        IQueryable<string> GetLocalesQuery();
    }
}
