using System.Text.Json.Serialization;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace Laquila.Integrations.Core.Domain.DTO.Prenota.Request
{
    public class PrenotaDTO
    {
        public PrenotaDTO(){}
        
        [JsonPropertyName("lo_ma_cnpj_owner")]
        public required string LoMaCnpjOwner { get; set; }

        [JsonPropertyName("lo_ma_cnpj")]
        public required string LoMaCnpj { get; set; }

        [JsonPropertyName("lo_ma_cnpj_carrier")]
        public string? LoMaCnpjCarrier { get; set; }

        [JsonPropertyName("lo_ma_cnpj_redespacho")]
        public string? LoMaCnpjRedespacho { get; set; }

        [JsonPropertyName("lo_priority")]
        public int LoPriority { get; set; }

        [JsonPropertyName("lo_type")]
        public int LoType { get; set; }

        [JsonPropertyName("lo_oe")]
        public long LoOe { get; set; }

        [JsonPropertyName("oe_erp_order")]
        public long OeErpOrder { get; set; }

        [JsonPropertyName("items")]
        public List<PrenotaItemsDTO> Items { get; set; } = new List<PrenotaItemsDTO>();
    }
}