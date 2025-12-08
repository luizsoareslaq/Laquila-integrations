using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Worker.Clients.Inbound
{
    public interface IInboundClient
    {
        Task<PagedResult<ReceiveInvoiceDTO>?> GetReceiveInvoicesAsync(LAQFilters filters, CancellationToken ct);
    }
}