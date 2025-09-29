using Laquila.Integrations.Core.Domain.DTO.Prenota.Response;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IPrenotaService
    {
        Task<PagedResult<List<PrenotaResumoDTO>>> GetPrenotasAsync(LAQFilters filters, CancellationToken ct);
    }
}