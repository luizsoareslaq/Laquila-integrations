using System.Text.Json.Serialization;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace Laquila.Integrations.Core.Domain.DTO.Prenota.Request
{
    public class PrenotaDTO
    {
        [JsonPropertyName("lo_ma_cnpj_owner")]
        [SwaggerSchema("CNPJ da Empresa (Laquila)")]
        public required string LoMaCnpjOwner { get; set; }

        [JsonPropertyName("lo_ma_cnpj")]
        [SwaggerSchema("CNPJ do cliente")]
        public required string LoMaCnpj { get; set; }

        [JsonPropertyName("lo_ma_cnpj_carrier")]
        [SwaggerSchema("CNPJ da transportadora")]
        public string? LoMaCnpjCarrier { get; set; }

        [JsonPropertyName("lo_ma_cnpj_redespacho")]
        [SwaggerSchema("CNPJ da transportadora redespacho")]
        public string? LoMaCnpjRedespacho { get; set; }

        [JsonPropertyName("lo_priority")]
        [SwaggerSchema("Prioridade do pedido")]
        public int LoPriority { get; set; }

        [JsonPropertyName("lo_type")]
        [SwaggerSchema("Tipo do pedido")]
        public int LoType { get; set; }

        [JsonPropertyName("lo_oe")]
        [SwaggerSchema("Id da prenota")]
        public long LoOe { get; set; }

        [JsonPropertyName("oe_erp_order")]
        [SwaggerSchema("Número da prenota no ERP")]
        public long OeErpOrder { get; set; }

        [JsonPropertyName("items")]
        [SwaggerSchema("Itens do pedido")]
        public List<PedidoItemsDTO> Items { get; set; } = new List<PedidoItemsDTO>();
    }
}