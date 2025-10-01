using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Response;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IPrenotaService
    {
        Task<PagedResult<PrenotaDTO>> GetPrenotasAsync(LAQFilters filters, CancellationToken ct);
    }
}