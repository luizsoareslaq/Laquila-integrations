using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Response;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<PagedResult<PrenotaDTO>> GetUnsentOrdersAsync(LAQFilters filters, CancellationToken ct);
        Task<ResponseDto> UpdateOrderStatusAsync(long lo_oe, PrenotaDatesDTO dto);
        Task<ResponseDto> UpdateRenouncedItemsAsync(long lo_oe, PrenotaRenouncedDTO dto);
    }
}