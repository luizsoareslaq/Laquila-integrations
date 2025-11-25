using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Shared;
using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request
{
    public class ReceiveInvoiceDTO
    {
        [JsonProperty("li_id")]
        public int LiId { get; set; }

        [JsonProperty("li_ma_cnpj_owner")]
        public required string LiMaCnpjOwner { get; set; }

        [JsonProperty("li_priority")]
        public int LiPriority { get; set; }

        [JsonProperty("fn_ma_cnpj")]
        public required string FnMaCnpj { get; set; }

        [JsonProperty("fn_inv_number")]
        public long FnInvNumber { get; set; }

        [JsonProperty("fn_type")]
        public int FnType { get; set; }

        [JsonProperty("fn_serial_nr")]
        public required string FnSerialNr { get; set; }

        [JsonProperty("items")]
        public List<ReceiveInvoiceItemsDTO> Items { get; set; } = null!;
    }
}