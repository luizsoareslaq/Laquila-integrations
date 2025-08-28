using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUserCompanies
    {
        public LaqApiUserCompanies(Guid userId, Guid companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }

        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("company_id")]
        public Guid CompanyId { get; set; }

        public virtual LaqApiUsers User { get; set; } = null!;
        public virtual LaqApiCompany Company { get; set; } = null!;
    }
}