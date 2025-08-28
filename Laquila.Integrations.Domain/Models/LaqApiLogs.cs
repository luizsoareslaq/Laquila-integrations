using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiLogs : BaseEntity
    {
        [Column("api_user_id")]
        public Guid ApiUserId { get; set; }
        [Column("method")]
        public string Method { get; set; }
        [Column("endpoint")]
        public string Endpoint { get; set; }
        [Column("request_payload")]
        public string RequestPayload { get; set; }
        [Column("response_payload")]
        public string ResponsePayload { get; set; }
        [Column("status_code")]
        public int StatusCode { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual LaqApiUsers User { get; set; }
    }
}