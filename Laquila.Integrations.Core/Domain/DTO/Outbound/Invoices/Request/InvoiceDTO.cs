using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;

namespace Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request
{
    public class InvoiceDTO
    {
        [JsonPropertyName("lo_ma_cnpj_owner")]
        public required string LoMaCnpjOwner { get; set; }

        [JsonPropertyName("lo_ma_cnpj")]
        public required string LoMaCnpj { get; set; }

        [JsonPropertyName("lo_ma_cnpj_carrier")]
        public string? LoMaCnpjCarrier { get; set; }

        [JsonPropertyName("lo_ma_cnpj_redespacho")]
        public string? LoMaCnpjRedespacho { get; set; }

        [JsonPropertyName("lo_oe")]
        public long LoOe { get; set; }

        [JsonPropertyName("oe_inv_number")]
        public long OeInvNumber { get; set; }
        [JsonPropertyName("oe_serial_nr")]
        public required string OeSerialNr { get; set; }

        [JsonPropertyName("items")]
        public List<InvoiceItemsDTO> Items { get; set; } = new List<InvoiceItemsDTO>();
    }
}