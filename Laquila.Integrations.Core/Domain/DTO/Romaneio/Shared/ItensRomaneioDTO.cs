using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared
{
    public class ItensRomaneioDTO
    {
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
        [JsonPropertyName("qt_baixa")]
        public decimal QtBaixa { get; set; } = 0;
        [JsonPropertyName("qt_cortada")]
        public decimal QtCortada { get; set; } = 0;
        [JsonPropertyName("qt_faturada")]
        public decimal QtFaturada { get; set; } = 0;
        [JsonPropertyName("mensagem")]
        public string? Mensagem { get; set; }

    }
}