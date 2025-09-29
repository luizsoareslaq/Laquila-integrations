using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface IPrenotaRepository
    {
        Task<(IEnumerable<VLAQ_BuscarPrenotasNaoIntegradas_WMS>, int TotalCount)> GetPrenotasAsync(LAQFilters filters, CancellationToken ct);
    }
}