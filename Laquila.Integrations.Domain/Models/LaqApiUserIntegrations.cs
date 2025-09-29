using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUserIntegrations
    {
        public LaqApiUserIntegrations(Guid userId, Guid integrationId)
        {
            UserId = userId;
            IntegrationId = integrationId;
        }

        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("api_integration_id")]
        public Guid IntegrationId { get; set; }

        public virtual LaqApiUsers User { get; set; } = null!;
        public virtual LaqApiIntegrations Integration { get; set; } = null!;
    }
}