using Laquila.Integrations.Application.DTO.ApiIntegration.Request;
using Laquila.Integrations.Application.DTO.ApiIntegration.Response;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IApiIntegrationsService
    {
        Task<ApiIntegrationResponseDTO> CreateApiIntegration(ApiIntegrationDTO dto);
        Task<(List<ApiIntegrationResponseDTO> Data, int DataCount)> GetApiIntegrations(int page, int pageSize, string orderBy, bool ascending);
        Task<ApiIntegrationResponseDTO> GetApiIntegrationById(Guid id);
        Task UpdateApiIntegration(Guid id, ApiIntegrationDTO dto);
    }
}