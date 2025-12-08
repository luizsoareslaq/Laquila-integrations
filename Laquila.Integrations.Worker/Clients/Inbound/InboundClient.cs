using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Worker.Infrastructure.Http;

namespace Laquila.Integrations.Worker.Clients.Inbound
{
    public class InboundClient : IInboundClient
    {
        private readonly IEverestHttpClient _http;

        public InboundClient(IEverestHttpClient http)
        {
            _http = http;
        }

        public Task<PagedResult<ReceiveInvoiceDTO>?> GetReceiveInvoicesAsync(LAQFilters filters, CancellationToken ct)
        {
            return _http.GetAsync<PagedResult<ReceiveInvoiceDTO>>(
                "/inbound/invoices",
                filters,
                ct
            );
        }
    }
}