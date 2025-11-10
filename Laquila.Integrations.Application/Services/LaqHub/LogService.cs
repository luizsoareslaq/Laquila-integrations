using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Services.LaqHub
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task HandleLogAsync(LaqApiLogs request)
        {
            await _logRepository.SaveLogAsync(request);
        }
    }
}