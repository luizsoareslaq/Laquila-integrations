using System.Text.Json.Serialization;

namespace Laquila.Integrations.Application.DTO.Auth.Request
{
    public class RefreshTokenRequestDto
    {
        public RefreshTokenRequestDto(string refreshToken)
        {
            this.RefreshToken = refreshToken;
        }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonIgnore]
        private string Document { get; set; } 
        
    }
}