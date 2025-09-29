using System.Text.Json.Serialization;

namespace Laquila.Integrations.Application.DTO.Auth.Response
{
    public class TokenResponseDto
    {
        public TokenResponseDto(string jwtToken, DateTime jwtTokenExpirationDate, string refreshToken, DateTime refreshTokenExpirationDate)
        {
            this.JWTToken = jwtToken;
            this.JWTTokenExpirationDate = jwtTokenExpirationDate;
            this.RefreshToken = refreshToken;
            this.RefreshTokenExpirationDate = refreshTokenExpirationDate;
        }
        [JsonPropertyName("jwt_token")]
        public string JWTToken { get; set; }
        [JsonPropertyName("jwt_token_expiration_date")]
        public DateTime JWTTokenExpirationDate { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("refresh_token_expiration_date")]
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}