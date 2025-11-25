using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IInboundService
    {
        Task<PagedResult<ReceiveInvoiceDTO>> GetUnsentReceiveInvoicesAsync(LAQFilters filters, CancellationToken ct);

        // Task<ResponseDto> UpdateOrderStatusAsync(long lo_oe, PrenotaDatesDTO dto);
        // Task<ResponseDto> UpdateRenouncedItemsAsync(long lo_oe, PrenotaRenouncedDTO dto);
        // Task<PagedResult<InvoiceDTO>> GetUnsentInvoicesAsync(LAQFilters filters, CancellationToken ct);
    }
}