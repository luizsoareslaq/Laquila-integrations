using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Invoices.Request
{
    public class InvoiceDatesDTO
    {
        [JsonPropertyName("lo_dt_ini_loading")]
        public DateTime? LoDtIniLoading { get; set; }

        [JsonPropertyName("lo_dt_end_loading")]
        public DateTime? LoDtEndLoading { get; set; }
    }
}