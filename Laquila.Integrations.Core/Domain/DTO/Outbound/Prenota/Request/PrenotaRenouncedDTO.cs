using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Prenota.Request
{
    public class PrenotaRenouncedDTO
    {
        [JsonPropertyName("items")]
        public required List<PrenotaItemsRenouncedDTO> Items { get; set; }
    }

    public class PrenotaItemsRenouncedDTO
    {
        [JsonPropertyName("oel_id")]
        public long OelId { get; set; }

        [JsonPropertyName("oel_qty_renounced")]
        public int OelQtyRenounced { get; set; } = 0;
    }
}