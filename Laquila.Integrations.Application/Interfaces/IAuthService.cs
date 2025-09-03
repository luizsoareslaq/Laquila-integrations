using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> GenerateToken(LaqApiUsers user);
        bool VerifyPassword(string password, string storedHash, string salt);
        Task<ClaimsPrincipal> ValidateToken(string token);
        Task<Guid> GetIdByJwt();
        Task<string> GetNameByJwt();
        Task<string> GetRoleByJwt();
        Task<TokenResponseDto> DoLoginAsync(LoginRequestDto dto);
        Task<object> RefreshTokenAsync(string refreshToken);
    }
}