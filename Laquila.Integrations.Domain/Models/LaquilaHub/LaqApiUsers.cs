using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiUsers : BaseEntity
    {
        public LaqApiUsers(string username, string hash, string salt, int statusId)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
            StatusId = statusId;
        }

        [Column("username")]
        public string Username { get; set; }
        [Column("hash")]
        public string Hash { get; set; }
        [Column("salt")]
        public string Salt { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [Column("disabled_at")]
        public DateTime? DisabledAt { get; set; }

        public virtual LaqApiStatus? Status { get; set; }
        public virtual ICollection<LaqApiAuthTokens> AuthTokens { get; set; } = new List<LaqApiAuthTokens>();
        public virtual ICollection<LaqApiUserRoles> UserRoles { get; set; } = new List<LaqApiUserRoles>();
        public virtual ICollection<LaqApiUserCompanies> UserCompanies { get; set; } = new List<LaqApiUserCompanies>();
        public virtual ICollection<LaqApiUserIntegrations> UserIntegrations { get; set; } = new List<LaqApiUserIntegrations>();
    }
}