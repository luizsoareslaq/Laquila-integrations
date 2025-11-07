using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Interfaces.LaqHub
{
    public interface ILogService
    {
        Task HandleLogAsync(LaqApiLogs request);
    }
}