using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiAuthTokens : BaseEntity
    {
        public LaqApiAuthTokens(Guid apiUserId, string accessToken, string refreshToken, DateTime expiresAt)
        {
            ApiUserId = apiUserId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }
        
        [Column("api_user_id")]
        public Guid ApiUserId { get; set; }
        [Column("access_token")]
        public string AccessToken { get; set; }
        [Column("refresh_token")]
        public string RefreshToken { get; set; }
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual LaqApiUsers User { get; set; } = null!;
    }
}