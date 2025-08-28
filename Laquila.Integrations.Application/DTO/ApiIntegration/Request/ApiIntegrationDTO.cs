
namespace Laquila.Integrations.Application.DTO.ApiIntegration.Request
{
    public class ApiIntegrationDTO
    {
        public ApiIntegrationDTO(string integrationName, int? statusId = null)
        {
            IntegrationName = integrationName;
            StatusId = statusId;
        }
        public string IntegrationName { get; set; }
        public int? StatusId { get; set; } = null;
        public List<Guid>? CompanyIds { get; set; } = null;
        public List<Guid>? UserIds { get; set; } = null;
    }
}