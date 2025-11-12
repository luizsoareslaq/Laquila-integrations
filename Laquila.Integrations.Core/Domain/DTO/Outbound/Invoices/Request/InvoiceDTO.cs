using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request
{
    public class InvoiceDTO
    {
        [SwaggerSchema("Cnpj da empresa responsável pelo pedido")]
        [JsonPropertyName("lo_ma_cnpj_owner")]
        public required string LoMaCnpjOwner { get; set; }

        [SwaggerSchema("Cnpj da empresa responsável pelo pedido")]
        [JsonPropertyName("lo_ma_cnpj")]
        public required string LoMaCnpj { get; set; }

        [JsonPropertyName("lo_ma_cnpj_carrier")]
        public string? LoMaCnpjCarrier { get; set; }

        [JsonPropertyName("lo_ma_cnpj_redespacho")]
        public string? LoMaCnpjRedespacho { get; set; }

        [JsonPropertyName("lo_oe")]
        public int LoOe { get; set; }

        [JsonPropertyName("oe_inv_number")]
        public long OeInvNumber { get; set; }
        [JsonPropertyName("oe_serial_nr")]
        public required string OeSerialNr { get; set; }

        [JsonPropertyName("items")]
        public List<InvoiceItemsDTO> Items { get; set; } = new List<InvoiceItemsDTO>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? OeId { get; set; }
    }
}