
namespace Laquila.Integrations.Application.DTO.ApiIntegration.Response
{
    public class ApiIntegrationResponseDTO
    {
        public ApiIntegrationResponseDTO(Guid id, string integrationName, string? status, Dictionary<Guid, string>? users, Dictionary<Guid, string>? companies = null)
        {
            Id = id;
            IntegrationName = integrationName;
            Status = status;
            Users = users ?? new Dictionary<Guid, string>();
            Companies = companies ?? new Dictionary<Guid, string>();
        }
        public Guid Id { get; set; }
        public string IntegrationName { get; set; }
        public string? Status { get; set; }
        public Dictionary<Guid, string> Users { get; set; } = new Dictionary<Guid, string>();
        public Dictionary<Guid, string> Companies { get; set; } = new Dictionary<Guid, string>();
    }
}