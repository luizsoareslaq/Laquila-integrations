using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiIntegrationCompanies
    {
        public LaqApiIntegrationCompanies(Guid companyId, Guid apiIntegrationId)
        {
            CompanyId = companyId;
            ApiIntegrationId = apiIntegrationId;
        }

        [Column("company_id")]
        public Guid CompanyId { get; set; }
        [Column("api_integration_id")]
        public Guid ApiIntegrationId { get; set; }

        public virtual LaqApiCompany Company { get; set; } = null!;
        public virtual LaqApiIntegrations Integration { get; set; } = null!;
    }
}