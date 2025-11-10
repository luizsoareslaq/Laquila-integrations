using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub
{
    public interface ILogRepository
    {
        Task SaveLogAsync(LaqApiLogs entity);
    }
}