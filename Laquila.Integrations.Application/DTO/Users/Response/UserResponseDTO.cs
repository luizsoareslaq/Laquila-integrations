
namespace Laquila.Integrations.Application.DTO.Users
{
    public class UserResponseDTO
    {
        public UserResponseDTO(Guid id, string username, string? status, Dictionary<Guid, string>? companies, Dictionary<Guid, string>? integrations, Dictionary<int, string>? roles)
        {
            this.Id = id;
            this.Username = username;
            this.Status = status;
            this.Companies = companies;
            this.Integrations = integrations;
            this.Roles = roles;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Status { get; set; }
        public Dictionary<Guid, string>? Companies { get; set; }
        public Dictionary<Guid, string>? Integrations { get; set; }
        public Dictionary<int, string>? Roles { get; set; }
    }
}