using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Helpers
{
    public class QueueHelper
    {
        public static LaqApiSyncQueue ModelToSyncQueue(string originTable, string originKey,string originValue, object dto, ApiStatusEnum ApiStatus, int attempCount = 1)
        {
            var queue = new LaqApiSyncQueue()
            {
                OriginTable = originTable,
                OriginKey = originKey,
                OriginValue = originValue,
                StatusId = (int)ApiStatus,
                Payload = System.Text.Json.JsonSerializer.Serialize(dto),
                CreatedAt = DateTime.UtcNow,
                AttempCount = attempCount
            };

            return queue;
        }
    }
}