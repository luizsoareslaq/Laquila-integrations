using System.Text.Json.Serialization;

namespace Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request
{
    public class InvoiceQualityAssuranceDTO
    {
        public required List<InvoiceQualityAssuranceItemsDTO> items { get; set; }
    }

    public class InvoiceQualityAssuranceItemsDTO
    {
        [JsonPropertyName("fnl_id")]
        public long FnlId { get; set; }

        [JsonPropertyName("fnl_qty_approved")]
        public int FnlQtyApproved { get; set; }

        [JsonPropertyName("fnl_qty_rejected")]
        public int FnlQtyRejected { get; set; }
    }
}