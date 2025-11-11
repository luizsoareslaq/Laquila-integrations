using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared
{
    public class PrenotaItemsDTO
    {
        [JsonPropertyName("oel_id")]
        [SwaggerSchema("ID único do item do pedido")]
        public int OelId { get; set; }

        [JsonPropertyName("oel_at_id")]
        [SwaggerSchema("SKU do item")]
        public required string OelAtId { get; set; }

        [JsonPropertyName("oel_qty_req")]
        [SwaggerSchema("Quantidade pedida do item")]
        public int OelQtyReq { get; set; }
    }
}