using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Prenota.Request
{
    public class PrenotaRenouncedDTO
    {
        [JsonProperty("items")]
        public required List<PrenotaItemsRenouncedDTO> Items { get; set; }
    }

    public class PrenotaItemsRenouncedDTO
    {
        [JsonProperty("oel_id")]
        public long OelId { get; set; }

        [JsonProperty("oel_qty_renounced")]
        public int OelQtyRenounced { get; set; } = 0;
    }
}