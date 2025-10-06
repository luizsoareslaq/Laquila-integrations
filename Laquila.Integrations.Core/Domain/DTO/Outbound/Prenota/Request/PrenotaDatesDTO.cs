using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Laquila.Integrations.Core.Domain.DTO.Romaneio.Request
{
    public class PrenotaDatesDTO
    {
        [JsonPropertyName("lo_dt_ini_picking")]
        public DateTime? LoDtIniPicking { get; set; }
        [JsonPropertyName("lo_dt_end_picking")]
        public DateTime? LoDtEndPicking { get; set; }
        [JsonPropertyName("lo_dt_ini_conf")]
        public DateTime? LoDtIniConf { get; set; }
        [JsonPropertyName("lo_dt_end_conf")]
        public DateTime? LoDtEndConf { get; set; }
    }
}