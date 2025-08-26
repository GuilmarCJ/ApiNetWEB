using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly InventarioContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(InventarioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest loginRequest)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.NombreUsuario == loginRequest.NombreUsuario &&
                                             u.Contraseña == loginRequest.Contraseña);

                if (usuario == null)
                    return null;

                var token = GenerateJwtToken(usuario);
                var sessionId = Guid.NewGuid().ToString();

                return new LoginResponse
                {
                    Token = token,
                    SessionId = sessionId,
                    Usuario = new UsuarioInfo
                    {
                        Id = usuario.Id,
                        NombreUsuario = usuario.NombreUsuario ?? string.Empty,
                        NombreCompleto = usuario.NombreCompleto ?? string.Empty,
                        LocalAsignado = usuario.LocalAsignado ?? string.Empty,
                        Rol = usuario.Rol?.Nombre ?? string.Empty
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Authenticate: {ex.Message}");
                throw;
            }
        }

        public async Task<object> DebugInfo()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var userCount = await _context.Usuarios.CountAsync();

                var users = await _context.Usuarios
                    .Include(u => u.Rol)
                    .Select(u => new {
                        u.Id,
                        u.NombreUsuario,
                        u.Contraseña,
                        u.NombreCompleto,
                        u.LocalAsignado,
                        RolId = u.RolId,
                        RolNombre = u.Rol != null ? u.Rol.Nombre : "NULL"
                    })
                    .ToListAsync();

                return new
                {
                    DatabaseConnected = canConnect,
                    UserCount = userCount,
                    Users = users
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message, stackTrace = ex.StackTrace };
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? ""),
                new Claim("SessionId", Guid.NewGuid().ToString())
            };

            // Agregar claims adicionales
            if (!string.IsNullOrEmpty(usuario.NombreCompleto))
                claims.Add(new Claim("FullName", usuario.NombreCompleto));

            if (!string.IsNullOrEmpty(usuario.LocalAsignado))
                claims.Add(new Claim("Local", usuario.LocalAsignado));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
