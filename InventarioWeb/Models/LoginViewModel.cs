namespace InventarioWeb.Models
{
    public class LoginViewModel
    {
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string SessionId { get; set; }
        public UsuarioInfo Usuario { get; set; }
    }

    public class UsuarioInfo
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string LocalAsignado { get; set; }
        public string Rol { get; set; }
    }

    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string MaterialCod { get; set; }
        public string Descripcion { get; set; }
        public string Almacen { get; set; }
        public string Lote { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Conteo { get; set; }
        public decimal? Reconteo { get; set; }
        public DateTime? FecReg { get; set; }
        public string Obs { get; set; }
        public string Local { get; set; }
        public string Umb { get; set; }
        public string Parihuela { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? Fec { get; set; }
        public string Cta { get; set; }
        public string Usuario { get; set; }
        public DateTime? FecSys { get; set; }
        public string Estado { get; set; }
    }

    public class MaterialesResponse
    {
        public List<MaterialViewModel> Materiales { get; set; } = new List<MaterialViewModel>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public string LocalAsignado { get; set; }
        public string Mensaje { get; set; }
        public string SessionId { get; set; }
    }
}
