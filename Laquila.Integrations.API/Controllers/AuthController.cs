using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var tokenResult = await _authService.RefreshTokenAsync(dto);
            return Ok(tokenResult);
        }

    }
}