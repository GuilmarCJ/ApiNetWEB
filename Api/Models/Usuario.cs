namespace Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string? NombreCompleto { get; set; }
        public string? LocalAsignado { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }

    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    public class LoginRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public UsuarioInfo Usuario { get; set; } = new UsuarioInfo();
    }

    public class UsuarioInfo
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string LocalAsignado { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }

    public class Material
    {
        public int Id { get; set; }
        public string? MaterialCod { get; set; }
        public string? Descripcion { get; set; }
        public string? Almacen { get; set; }
        public string? Lote { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Conteo { get; set; }
        public decimal? Reconteo { get; set; }
        public DateTime? FecReg { get; set; }
        public string? Obs { get; set; }
        public string? Local { get; set; }
        public string? Umb { get; set; }
        public string? Parihuela { get; set; }
        public string? Ubicacion { get; set; }
        public DateTime? Fec { get; set; }
        public string? Cta { get; set; }
        public string? Usuario { get; set; }
        public DateTime? FecSys { get; set; }
        public string? Estado { get; set; }
    }

    public class MaterialRequest
    {
        public string? Local { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class MaterialResponse
    {
        public List<Material> Materiales { get; set; } = new List<Material>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string LocalAsignado { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty; // Agregar esta propiedad
    }
}