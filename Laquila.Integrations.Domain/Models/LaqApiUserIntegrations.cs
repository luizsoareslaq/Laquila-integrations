using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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