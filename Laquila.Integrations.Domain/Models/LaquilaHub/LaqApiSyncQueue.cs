using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiSyncQueue : BaseEntity
    {
        [Column("origin_table")]
        public string OriginTable { get; set; }
        [Column("origin_key")]
        public string OriginKey { get; set; }
        [Column("origin_value")]
        public string OriginValue { get; set; }
        [Column("company_cnpj")]
        public string? CompanyCnpj { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("payload")]
        public string Payload { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }
        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; }
        [Column("attemp_count")]
        public int AttempCount { get; set; }

        public LaqApiStatus? Status { get; set; }
    }
}