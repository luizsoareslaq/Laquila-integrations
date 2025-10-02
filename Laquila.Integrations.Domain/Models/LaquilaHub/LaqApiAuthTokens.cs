using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiAuthTokens : BaseEntity
    {
        public LaqApiAuthTokens(Guid apiUserId, string accessToken, string refreshToken, DateTime accessTokenExpiresAt, DateTime refreshTokenExpiresAt, string companyCnpj)
        {
            ApiUserId = apiUserId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
            CreatedAt = DateTime.UtcNow;
            CompanyCnpj = companyCnpj;
        }

        [Column("api_user_id")]
        public Guid ApiUserId { get; set; }
        [Column("access_token")]
        public string AccessToken { get; set; }
        [Column("refresh_token")]
        public string RefreshToken { get; set; }
        [Column("access_token_expires_at")]
        public DateTime AccessTokenExpiresAt { get; set; }
        [Column("refresh_token_expires_at")]
        public DateTime RefreshTokenExpiresAt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("company_cnpj")]
        public string CompanyCnpj { get; set; }

        public virtual LaqApiUsers User { get; set; } = null!;
    }
}