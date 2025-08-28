using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiRoles
    {
        public LaqApiRoles(int roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }

        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("role_name")]
        public string RoleName { get; set; }
        [Column("description")]
        public string? Description { get; set; }

        public ICollection<LaqApiUserRoles> UserRoles { get; set; }
    }
}