using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface INotaRepository
    {
        Task<(IEnumerable<VLAQConsultarNotas>,int TotalCount)> GetNotasAsync(LAQFilters filters, CancellationToken ct);
    }
}