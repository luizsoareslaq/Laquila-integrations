using System.Security.Claims;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.LaqHubControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto dto)
        {
            var token = await _authService.DoLoginAsync(dto);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("/me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst("id")?.Value;
            var nome = User.FindFirst("nome")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var isAdmin = roles.Contains("Admin");

            return Ok(new
            {
                IdUsuario = userId,
                Nome = nome,
                Email = email,
                Role = roles,
                IsAdmin = isAdmin
            });
        }
    }
}