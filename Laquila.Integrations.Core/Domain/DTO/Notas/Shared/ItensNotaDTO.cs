using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.DTO.Notas.Shared
{
    public class ItensNotaDTO
    {
        [JsonPropertyName("nr_ordem")]
        public int NrOrdem{ get; set; }
        [JsonPropertyName("cd_item")]
        public required string CdItem { get; set; }
        [JsonPropertyName("ds_item")]
        public required string DsItem { get; set; }
        [JsonPropertyName("id_embalagem")]
        public int IdEmbalagem { get; set; }
        [JsonPropertyName("ds_embalagem")]
        public required string DsEmbalagem { get; set; }
        [JsonPropertyName("qt_item")]
        public decimal QtItem { get; set; } = 0;
        [JsonPropertyName("vl_total_item")]
        public decimal VlTotalItem { get; set; } = 0;
    }
}