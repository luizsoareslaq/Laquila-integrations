using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Worker.Infrastructure.Auth
{
    public interface IEverestAuthService
    {
        Task<string> GetValidTokenAsync(CancellationToken ct);
    }
}