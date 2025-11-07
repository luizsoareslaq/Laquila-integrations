using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Enums;

namespace Laquila.Integrations.Application.Interfaces.LaqHub
{
    public interface IQueueService
    {
        public Task<Guid> EnqueueAsync(string originTable, string originKey, string originId, object payload, ApiStatusEnum status, int attempCount, string? errorMessage);
    }
}