using Laquila.Integrations.Application.DTO.ApiIntegration.Request;
using Laquila.Integrations.Application.DTO.ApiIntegration.Response;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Application.Services
{
    public class ApiIntegrationsService : IApiIntegrationsService
    {
        private readonly IApiIntegrationsRepository _apiIntegrationsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApiIntegrationsService(IApiIntegrationsRepository apiIntegrationsRepository
                                    , IUserRepository userRepository
                                    , ICompanyRepository companyRepository
                                    , IUnitOfWork unitOfWork)
        {
            _apiIntegrationsRepository = apiIntegrationsRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiIntegrationResponseDTO> CreateApiIntegration(ApiIntegrationDTO dto)
        {
            if (await _apiIntegrationsRepository.ApiIntegrationsExistsAsync(dto.IntegrationName, Guid.Empty))
                throw new BadRequestException("API Integration name already exists.");

            var apiIntegration = new LaqApiIntegrations(dto.IntegrationName, 1); // Default status to Active (1)

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = await _apiIntegrationsRepository.CreateApiIntegration(apiIntegration);

                await _unitOfWork.CommitAsync();

                return new ApiIntegrationResponseDTO(entity.Id
                                                   , entity.IntegrationName
                                                   , entity.Status?.Description ?? null
                                                   , null
                                                   , null
                                                 );
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }
        }

        public async Task<(List<ApiIntegrationResponseDTO> Data, int DataCount)> GetApiIntegrations(int page, int pageSize, string orderBy, bool ascending)
        {
            var (entities, count) = await _apiIntegrationsRepository.GetApiIntegrations(page, pageSize, orderBy, ascending);

            return (entities.Select(entity => new ApiIntegrationResponseDTO(
                entity.Id,
                entity.IntegrationName,
                entity.Status?.Description ?? null,
                entity.UserIntegrations?.ToDictionary(ui => ui.UserId, ui => ui.User.Username) ?? new Dictionary<Guid, string>(),
                entity.IntegrationCompanies?.ToDictionary(ic => ic.CompanyId, ic => ic.Company?.CompanyName ?? string.Empty) ?? new Dictionary<Guid, string>()
            )).ToList(), count);
        }

        public async Task<ApiIntegrationResponseDTO> GetApiIntegrationById(Guid id)
        {
            var entity = await _apiIntegrationsRepository.GetApiIntegrationById(id);

            return new ApiIntegrationResponseDTO(
                entity.Id,
                entity.IntegrationName,
                entity.Status?.Description ?? null,
                entity.UserIntegrations?.ToDictionary(ui => ui.UserId, ui => ui.User.Username) ?? new Dictionary<Guid, string>(),
                entity.IntegrationCompanies?.ToDictionary(ic => ic.CompanyId, ic => ic.Company?.CompanyName ?? string.Empty) ?? new Dictionary<Guid, string>()
            );
        }

        public async Task UpdateApiIntegration(Guid id, ApiIntegrationDTO dto)
        {
            if (await _apiIntegrationsRepository.ApiIntegrationsExistsAsync(dto.IntegrationName, id))
                throw new BadRequestException("API Integration with the same name already exists.");

            var apiIntegration = await _apiIntegrationsRepository.GetApiIntegrationById(id);

            // ===== USERS =====
            var actualUserIds = apiIntegration.UserIntegrations?
                .Select(ui => ui.UserId).ToHashSet() ?? new();

            var existingUserIds = await _userRepository.GetAllUserIds();
            var requestedUserIds = dto.UserIds?.ToHashSet() ?? new();

            var validUserIds = requestedUserIds.Intersect(existingUserIds).ToHashSet();

            var toAddUserIds = validUserIds.Except(actualUserIds);
            var toRemoveUserIds = actualUserIds.Except(validUserIds);

            // ===== COMPANIES =====
            var actualCompanyIds = apiIntegration.IntegrationCompanies?
                .Select(ic => ic.CompanyId).ToHashSet() ?? new();

            var existingCompanyIds = await _companyRepository.GetAllCompanyIds();
            var requestedCompanyIds = dto.CompanyIds?.ToHashSet() ?? new();

            var validCompanyIds = requestedCompanyIds.Intersect(existingCompanyIds).ToHashSet();

            var toAddCompanyIds = validCompanyIds.Except(actualCompanyIds);
            var toRemoveCompanyIds = actualCompanyIds.Except(validCompanyIds);

            apiIntegration.IntegrationName = dto.IntegrationName;
            apiIntegration.ModifiedAt = DateTime.UtcNow;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (toRemoveUserIds.Any() || toRemoveCompanyIds.Any())
                {
                    var usersToRemove = apiIntegration.UserIntegrations?
                        .Where(ui => toRemoveUserIds.Contains(ui.UserId))
                        .ToList() ?? new();

                    var companiesToRemove = apiIntegration.IntegrationCompanies?
                        .Where(ic => toRemoveCompanyIds.Contains(ic.CompanyId))
                        .ToList() ?? new();

                    await _apiIntegrationsRepository.RemoveJoinedIntegrationTables(usersToRemove, companiesToRemove);
                }

                if (toAddUserIds.Any())
                {
                    var userIntegrations = toAddUserIds.Select(userId => new LaqApiUserIntegrations(userId, apiIntegration.Id)).ToList();
                    await _apiIntegrationsRepository.AddUserIntegrations(userIntegrations);
                }

                if (toAddCompanyIds.Any())
                {
                    var companyIntegrations = toAddCompanyIds.Select(companyId => new LaqApiIntegrationCompanies(companyId, apiIntegration.Id)).ToList();
                    await _apiIntegrationsRepository.AddIntegrationCompanies(companyIntegrations);
                }

                await _apiIntegrationsRepository.UpdateApiIntegration(apiIntegration);
                
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }
        }
    }
}