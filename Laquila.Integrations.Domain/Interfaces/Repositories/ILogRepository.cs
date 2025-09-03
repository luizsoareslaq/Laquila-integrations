using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface ILogRepository
    {
        Task SaveLogAsync(LaqApiLogs entity);
    }
}