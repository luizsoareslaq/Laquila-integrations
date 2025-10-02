using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUrlIntegrations : BaseEntity
    {
        [Column("api_user_id")]
        public Guid ApiUserId { get; set; }
        [Column("api_integration_id")]
        public Guid ApiIntegrationId { get; set; }
        [Column("auth_type")]
        public string AuthType { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("request_body")]
        public string RequestBody { get; set; }
        [Column("response_body")]
        public string ResponseBody { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }

        public virtual LaqApiUsers User { get; set; }
        public virtual LaqApiIntegrations Integrations { get; set; }
        public virtual LaqApiStatus Status { get; set; }
    }
}