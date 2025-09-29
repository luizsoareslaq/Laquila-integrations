using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.DTO.MasterData.Items
{
    public class MasterDataItemsRequestDTO
    {
        [JsonPropertyName("items")]
        public required List<ItemsDetailsDTO> Items { get; set; }
    }
    public class ItemsDetailsDTO
    {
        [JsonPropertyName("at_id")]
        public required string AtId { get; set; }

        [JsonPropertyName("at_desc")]
        public required string AtDesc { get; set; }

        [JsonPropertyName("at_type")]
        public required string AtType { get; set; }
    }
}