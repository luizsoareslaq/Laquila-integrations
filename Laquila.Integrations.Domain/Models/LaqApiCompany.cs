using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiCompany : BaseEntity
    {
        public LaqApiCompany(int erpCode, string companyName, string document, int statusId)
        {
            ErpCode = erpCode;
            CompanyName = companyName;
            Document = document;
            StatusId = statusId;
        }

        [Column("erp_code")]
        public int ErpCode { get; set; }
        [Column("company_name")]
        public string CompanyName { get; set; }
        [Column("document")]
        public string Document { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }

        public virtual LaqApiStatus Status { get; set; } = null!;
        public virtual ICollection<LaqApiUserCompanies> UserCompanies { get; set; } = new List<LaqApiUserCompanies>();
        public virtual ICollection<LaqApiIntegrationCompanies> CompaniesIntegration { get; set; } = new List<LaqApiIntegrationCompanies>();
    }
}