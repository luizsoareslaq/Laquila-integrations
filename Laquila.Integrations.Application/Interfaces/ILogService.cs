using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface ILogService
    {
        Task HandleLogAsync(LaqApiLogs request);
    }
}