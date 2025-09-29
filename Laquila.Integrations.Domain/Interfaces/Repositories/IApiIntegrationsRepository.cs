using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface IApiIntegrationsRepository
    {
        Task<LaqApiIntegrations> CreateApiIntegration(LaqApiIntegrations apiIntegration);
        Task<(List<LaqApiIntegrations> Data, int DataCount)> GetApiIntegrations(int page, int pageSize, string orderBy, bool ascending);
        Task<LaqApiIntegrations> GetApiIntegrationById(Guid id);
        Task<LaqApiIntegrations> UpdateApiIntegration(LaqApiIntegrations apiIntegration);

        Task<bool> ApiIntegrationsExistsAsync(string integrationName, Guid id);
        Task<List<Guid>> GetAllIntegrationIds();

        Task RemoveJoinedIntegrationTables(List<LaqApiUserIntegrations> userIntegrations, List<LaqApiIntegrationCompanies> integrationCompanies);
        Task AddIntegrationCompanies(List<LaqApiIntegrationCompanies> integrationCompanies);
        Task AddUserIntegrations(List<LaqApiUserIntegrations> userIntegrations);
    }
}