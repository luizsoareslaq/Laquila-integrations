using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Enums
{
    public enum ApiStatus
    {
        Active = 1,
        Disabled = 2,
        Pending = 3,
        Processing = 4,
        Error = 5,
        Completed = 6,
        Expired = 7,
        Canceled = 8
    }
}