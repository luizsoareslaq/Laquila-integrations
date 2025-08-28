using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Filters
{
    public class UserFilters
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public int? StatusId { get; set; }
        public List<Guid>? Companies { get; set; }
        public List<Guid>? Integrations { get; set; }
        public List<int>? Roles { get; set; }
    }
}