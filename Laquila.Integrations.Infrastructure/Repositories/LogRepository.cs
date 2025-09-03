using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LaquilaHubContext _context;
        public LogRepository(LaquilaHubContext context)
        {
            _context = context;
        }
        public async Task SaveLogAsync(LaqApiLogs entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}