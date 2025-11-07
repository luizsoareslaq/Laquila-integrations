using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly LaquilaHubContext _context;
        public QueueRepository(LaquilaHubContext context)
        {
            _context = context;
        }
        public async Task<(List<LaqApiSyncQueue>, int totalCount)> GetQueueAsyncByFilters(int page, int pageSize, QueueFilters filters)
        {
            var query = _context.LaqApiSyncQueues.AsQueryable();

            if (filters.Id != null)
            {
                query = query.Where(q => q.Id == filters.Id);
            }

            if (filters.OriginTable != null)
            {
                query = query.Where(q => q.OriginTable == filters.OriginTable);
            }

            if (filters.OriginKey != null)
            {
                query = query.Where(q => q.OriginKey == filters.OriginKey);
            }

            if (filters.StatusId != null)
            {
                query = query.Where(q => q.StatusId == filters.StatusId);
            }

            // if (filters.CreatedAt != null)
            // {
            //     query = query.Where(q => q.CreatedAt == filters.CreatedAt);
            // }

            if (filters.AttempCount != null)
            {
                query = query.Where(q => q.AttempCount == filters.AttempCount);
            }

            var totalItems = await query.CountAsync();

            var items = await query
                                        .OrderBy(x=> x.CreatedAt)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .Include(x => x.Status)
                                        .ToListAsync() ?? throw new NotFoundException(MessageProvider.Get("QueuesNotFound", UserContext.Language));


            return (items, totalItems);
        }

        public async Task<LaqApiSyncQueue?> GetQueueAsyncById(Guid id)
        {
            var queue = await _context.LaqApiSyncQueues.FirstOrDefaultAsync(q => q.Id == id) ?? throw new NotFoundException(MessageProvider.Get("QueueIDNotFound", UserContext.Language));
            return queue;
        }

        public async Task<LaqApiSyncQueue> InsertQueueAsync(LaqApiSyncQueue queue)
        {
            _context.LaqApiSyncQueues.Add(queue);
            await _context.SaveChangesAsync();
            return queue;
        }
    }
}