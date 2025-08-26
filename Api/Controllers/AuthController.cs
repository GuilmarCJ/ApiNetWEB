using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.Authenticate(loginRequest);

            if (response == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            return Ok(response);
        }

        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            var debugInfo = await _authService.DebugInfo();
            return Ok(debugInfo);
        }
    }


}
