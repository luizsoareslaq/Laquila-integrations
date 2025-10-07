using System.Text.Json.Serialization;

namespace Laquila.Integrations.Application.DTO.Auth.Response
{
    public class TokenResponseDto
    {
        public TokenResponseDto(string jwtToken, int expiresIn)
        {
            this.JWTToken = jwtToken;
            this.ExpiresIn = expiresIn;
        }
        [JsonPropertyName("jwt_token")]
        public string JWTToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        // [JsonPropertyName("refresh_token")]
        // public string RefreshToken { get; set; }
        // [JsonPropertyName("refresh_token_expiration_date")]
        // public DateTime RefreshTokenExpirationDate { get; set; }
    }
}