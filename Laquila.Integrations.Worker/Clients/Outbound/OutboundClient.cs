using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Worker.Infrastructure.Http;

namespace Laquila.Integrations.Worker.Clients.Outbound
{
    public class OutboundClient : IOutboundClient
    {
        private readonly IEverestHttpClient _http;
        public OutboundClient(IEverestHttpClient client)
        {
            _http = client;
        }
        
        public async Task<PagedResult<PrenotaDTO>?> GetOrdersAsync(LAQFilters filters, CancellationToken ct)
        {
            return await _http.GetAsync<PagedResult<PrenotaDTO>>(
                "/outbound/orders",
                filters,
                ct
            );
        }

        public async Task<ResponseDto?> SendOrdersAsync(PrenotaDTO dto, Guid apiIntegrationId, CancellationToken ct)
        {
            return await _http.PostAsync<ResponseDto>(
                $"/outbound/orders/{apiIntegrationId}",
                dto,
                ct
            );
        }

    }
}