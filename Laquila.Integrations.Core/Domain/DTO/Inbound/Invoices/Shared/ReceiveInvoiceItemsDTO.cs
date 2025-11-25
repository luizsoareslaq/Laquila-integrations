using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Shared
{
    public class ReceiveInvoiceItemsDTO
    {
        [JsonProperty("fnl_id")]
        public long FnlId { get; set; }

        [JsonProperty("fnl_at_id")]
        public required string FnlAtId { get; set; }

        [JsonProperty("fnl_qty")]
        public int FnlQty { get; set; }
    }
}