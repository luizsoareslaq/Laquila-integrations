using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request
{
    public class ReceiveInvoiceDatesDTO
    {
        [JsonProperty("dt_ini_unload")]
        public DateTime? DtIniUnload { get; set; }

        [JsonProperty("dt_end_unload")]
        public DateTime? DtEndUnload { get; set; }
    }
}