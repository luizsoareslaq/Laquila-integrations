using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Worker.Querys.Interfaces
{
    public interface IEverest30Query
    {
        Task<PagedResult<PrenotaDTO>> GetOrders(LAQFilters filters, CancellationToken ct);
        Task<ResponseDto> SendOrders(PrenotaDTO dto, CancellationToken ct, Guid apiIntegrationId);
    }
}