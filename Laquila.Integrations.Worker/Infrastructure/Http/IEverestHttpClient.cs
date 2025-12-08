using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Worker.Infrastructure.Http
{
    public interface IEverestHttpClient
    {
        Task<T?> GetAsync<T>(string endpoint, object? query = null, CancellationToken ct = default);
        Task<T?> PostAsync<T>(string endpoint, object? body, CancellationToken ct = default);
        Task<T?> PatchAsync<T>(string endpoint, object? body, CancellationToken ct = default);
        Task<T?> PutAsync<T>(string endpoint, object? body, CancellationToken ct = default);
    }
}