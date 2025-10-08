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
        public static LaqApiSyncQueue ModelToSyncQueue(string originTable
                                                    , string originKey
                                                    , string originValue
                                                    , object dto
                                                    , ApiStatusEnum ApiStatus
                                                    , string? companyCnpj
                                                    , string? errorMessage)
        {
            var queue = new LaqApiSyncQueue()
            {
                OriginTable = originTable,
                OriginKey = originKey,
                OriginValue = originValue,
                CompanyCnpj = companyCnpj,
                StatusId = (int)ApiStatus,
                Payload = System.Text.Json.JsonSerializer.Serialize(dto),
                CreatedAt = DateTime.UtcNow,
                AttempCount = 1
            };

            if (!string.IsNullOrEmpty(errorMessage))
                queue.ErrorMessage = errorMessage;

            return queue;
        }
    }
}