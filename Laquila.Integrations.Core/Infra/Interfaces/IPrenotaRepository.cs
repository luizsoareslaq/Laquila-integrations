using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface IPrenotaRepository
    {
        Task<(IEnumerable<VMWMS_BuscarPrenotasNaoIntegradas>, int TotalCount)> GetPrenotasAsync(LAQFilters filters, CancellationToken ct);
    }
}