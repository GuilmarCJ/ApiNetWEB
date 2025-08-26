using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaterialesController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMateriales([FromQuery] MaterialRequest request)
        {
            try
            {
                // Obtener información del usuario del token JWT
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var localAsignado = User.FindFirst("Local")?.Value;
                var nombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
                var sessionId = User.FindFirst("SessionId")?.Value;

                // Validar que el usuario tenga un rol válido
                if (string.IsNullOrEmpty(userRole))
                {
                    return Unauthorized(new { message = "Token inválido o sin rol asignado" });
                }

                var materiales = await _materialService.GetMateriales(
                    userRole,
                    localAsignado,
                    nombreUsuario,
                    request
                );

                // Agregar información de la sesión
                materiales.SessionId = sessionId;

                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("locales")]
        public async Task<IActionResult> GetLocales()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var localAsignado = User.FindFirst("Local")?.Value;

                if (userRole == "Inventariador" && !string.IsNullOrEmpty(localAsignado))
                {
                    return Ok(new { locales = new List<string> { localAsignado } });
                }

                var locales = await _materialService.GetLocalesQuery().ToListAsync();
                return Ok(new { locales });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
