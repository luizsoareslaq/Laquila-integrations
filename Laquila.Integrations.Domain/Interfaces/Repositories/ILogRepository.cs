using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface ILogRepository
    {
        Task SaveLogAsync(LaqApiLogs entity);
    }
}