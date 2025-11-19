using System.Text.Json.Serialization;

namespace Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request
{
    public class InvoiceItemStatusDTO
    {
        public required List<InvoiceItemStatusItemsDTO> items { get; set; }
    }

    public class InvoiceItemStatusItemsDTO
    {
        [JsonPropertyName("fnl_id")]
        public long FnlId { get; set; }

        [JsonPropertyName("fnl_qty_confirmed")]
        public int FnlQtyConfirmed { get; set; }
    }
}