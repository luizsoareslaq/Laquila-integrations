using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUserRoles
    {
        public LaqApiUserRoles(Guid userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }

        public virtual LaqApiUsers User { get; set; }
        public virtual LaqApiRoles Role { get; set; }
    }
}