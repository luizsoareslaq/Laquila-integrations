using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface ILogService
    {
        Task HandleLogAsync(LaqApiLogs request);
    }
}