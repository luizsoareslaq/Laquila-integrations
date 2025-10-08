using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Helpers;
using Laquila.Integrations.Domain.Interfaces.Repositories;

namespace Laquila.Integrations.Application.Services
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _queueRepository;
        public QueueService(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }
        public async Task<Guid> EnqueueAsync(string originTable, string originKey, string originId, object payload, ApiStatusEnum status, int attempCount,string? errorMessage)
        {
            var queue = QueueHelper.ModelToSyncQueue(originTable, originKey, originId, payload, status, UserContext.CompanyCnpj,errorMessage);
            await _queueRepository.InsertQueueAsync(queue);

            return queue.Id;
        }
    }
}