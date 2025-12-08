using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Worker.Clients.Outbound
{
    public interface IOutboundClient
    {
        Task<PagedResult<PrenotaDTO>> GetOrdersAsync(LAQFilters filters, CancellationToken ct);
        Task<ResponseDto?> SendOrdersAsync(PrenotaDTO dto, Guid apiIntegrationId, CancellationToken ct);
    }
}