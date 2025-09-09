using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface IRomaneioRepository
    {
        Task<(IEnumerable<VLAQConsultarRomaneioItens>,int TotalCount)> GetRomaneiosAsync(LAQFilters filters, CancellationToken ct);
    }
}