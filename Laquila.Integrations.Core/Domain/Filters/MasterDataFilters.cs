using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.Filters
{
    public class MasterDataFilters
    {
        public int PageSize { get; set; } = 100;
        public List<string>? Items { get; set; }
    }
}