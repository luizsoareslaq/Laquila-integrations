using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUrlIntegrations : BaseEntity
    {
        [Column("api_integration_id")]
        public Guid ApiIntegrationId { get; set; }

        [Column("auth_type")]
        public required string AuthType { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("endpoint_key")]
        public required string EndpointKey { get; set; }

        [Column("url")]
        public required string Url { get; set; }

        [Column("type")]
        public required string Type { get; set; }

        [Column("request_body")]
        public required string RequestBody { get; set; }

        [Column("response_body")]
        public required string ResponseBody { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }

        // [NotMapped]
        // [JsonIgnore]
        // public virtual LaqApiIntegrations Integrations { get; set; }
        // [NotMapped]
        // [JsonIgnore]
        // public virtual LaqApiStatus Status { get; set; }
    }
}