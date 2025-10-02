using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface IQueueRepository
    {
        Task<LaqApiSyncQueue> InsertQueueAsync(LaqApiSyncQueue queue);
        Task<LaqApiSyncQueue?> GetQueueAsyncById(Guid id);
        Task<(List<LaqApiSyncQueue>,int totalCount)> GetQueueAsyncByFilters(int page, int pageSize, QueueFilters filters);
    }
}