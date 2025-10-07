using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared
{
    public class InvoiceItemsDTO
    {
        [JsonPropertyName("oel_id")]
        public long OelId { get; set; }

        [JsonPropertyName("oel_at_id")]
        public required string OelAtId { get; set; }

        [JsonPropertyName("oel_qty_atend")]
        public int OelQtyAtend { get; set; }
    }
}