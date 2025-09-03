using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Application.DTO.Users.Request
{
    public class UserDTO
    {
        public UserDTO(string username, string password, string confirmPassword,int? statusId, List<Guid>? companies, List<Guid>? integrations,List<int> roles)
        {
            Username = username;
            Password = password;
            ConfirmPassword = confirmPassword;
            StatusId = statusId;
            Companies = companies;
            Integrations = integrations;
            Roles = roles;
        }

        public string Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public int? StatusId { get; set; }
        public List<Guid>? Companies { get; set; }
        public List<Guid>? Integrations { get; set; }
        public List<int> Roles { get; set; }
    }
}