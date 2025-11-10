using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub
{
    public interface IAuthRepository
    {
        Task<LaqApiAuthTokens?> GetRefreshTokenAsync(string token);
        Task SaveTokenAsync(LaqApiAuthTokens token);
        Task DeleteTokenAsync(LaqApiAuthTokens token);
        Task DisableActiveTokens(Guid apiUserId, string access_token);
    }
}