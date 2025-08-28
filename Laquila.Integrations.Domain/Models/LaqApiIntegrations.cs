using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiIntegrations : BaseEntity
    {
        public LaqApiIntegrations(string integrationName, int statusId)
        {
            IntegrationName = integrationName;
            StatusId = statusId;
            CreatedAt = DateTime.UtcNow;
        }

        [Column("integration_name")]
        public string IntegrationName { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }

        public virtual LaqApiStatus? Status { get; set; }
        public virtual ICollection<LaqApiUserIntegrations> UserIntegrations { get; set; } = new List<LaqApiUserIntegrations>();
        public virtual ICollection<LaqApiIntegrationCompanies> IntegrationCompanies { get; set; } = new List<LaqApiIntegrationCompanies>();
        public virtual ICollection<LaqApiUrlIntegrations> ApiUrls { get; set; } = new List<LaqApiUrlIntegrations>();


    }
}