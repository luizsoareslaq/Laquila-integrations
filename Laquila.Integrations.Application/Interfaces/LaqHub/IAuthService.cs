using System.Security.Claims;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Interfaces.LaqHub
{
    public interface IAuthService
    {
        Task<TokenResponseDto> GenerateToken(LaqApiUsers user, string companyCnpj, string language);
        bool VerifyPassword(string password, string storedHash, string salt);
        ClaimsPrincipal ValidateToken(string token);
        Guid GetIdByJwt();
        string GetNameByJwt();
        string GetRoleByJwt();
        Task<TokenResponseDto> DoLoginAsync(LoginRequestDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

    }
}