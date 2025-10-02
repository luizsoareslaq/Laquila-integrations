using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Filters
{
    public class QueueFilters
    {
        public Guid? Id { get; set; }
        public string? OriginTable { get; set; }
        public string? OriginKey { get; set; }
        public string? OriginValue { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? AttempCount { get; set; }
    }
}