using System.Text.Json.Serialization;

namespace Laquila.Integrations.Application.DTO.Auth.Request
{
    public class LoginRequestDto
    {
        public LoginRequestDto(string username, string password, string cnpj)
        {
            this.Username = username;
            this.Password = password;
            this.Cnpj = cnpj;
        }

        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("company")]
        public string? Cnpj { get; set; }
        
    }
}