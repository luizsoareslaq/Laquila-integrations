using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Worker.Services.Interfaces
{
    public interface IProcessService
    {
        Task ProcessAsync(CancellationToken ct);
    }
}