using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Services
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