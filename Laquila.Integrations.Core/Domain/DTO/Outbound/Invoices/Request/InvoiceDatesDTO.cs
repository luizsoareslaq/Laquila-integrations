using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Invoices.Request
{
    public class InvoiceDatesDTO
    {
        [JsonProperty("lo_dt_ini_loading")]
        public DateTime? LoDtIniLoading { get; set; }

        [JsonProperty("lo_dt_end_loading")]
        public DateTime? LoDtEndLoading { get; set; }
    }
}