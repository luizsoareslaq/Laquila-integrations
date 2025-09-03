using System.Security.Cryptography.X509Certificates;
using Laquila.Integrations.Application.DTO.Users;
using Laquila.Integrations.Application.DTO.Users.Request;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Helpers;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IApiIntegrationsRepository _integrationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository
                        , ICompanyRepository companyRepository
                        , IApiIntegrationsRepository apiIntegrationsRepository
                        , IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _integrationRepository = apiIntegrationsRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserResponseDTO> CreateUserAsync(UserDTO dto)
        {
            if (await _userRepository.UsernameExistsAsync(dto.Username))
                throw new BadRequestException("Username already exists.");

            if (dto.Password != dto.ConfirmPassword)
                throw new BadRequestException("Password and Confirm Password do not match.");

            var salt = UserHelper.GenerateSalt();
            var hashedPassword = UserHelper.HashPassword(dto.Password, salt);

            var user = new LaqApiUsers(dto.Username, hashedPassword, salt, statusId: 1);

            dto.StatusId = 1;

            //Roles
            var roles = await _userRepository.GetRolesAsync();

            if (dto.Roles == null || !dto.Roles.Any())
                throw new BadRequestException("At least one role must be assigned to the user.");

            var rolesResult = roles.Where(r => dto.Roles.Contains(r.RoleId)).Distinct().ToList();

            //Companies
            var companies = await _companyRepository.GetAllCompanyIds();
            var companiesResult = dto.Companies != null ? companies.Where(c => dto.Companies.Contains(c)).Distinct().ToList() : new List<Guid>();

            //Integrations
            var integrations = await _integrationRepository.GetAllIntegrationIds();
            var integrationsResult = dto.Integrations != null ? integrations.Where(i => dto.Integrations.Contains(i)).Distinct().ToList() : new List<Guid>();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var createdUser = await _userRepository.CreateUserAsync(user);
                var userRoles = rolesResult.Select(r => new LaqApiUserRoles(createdUser.Id, r.RoleId)).ToList();
                if (userRoles.Count > 0)
                    await _userRepository.AddUserRoles(userRoles);
                else
                    throw new BadRequestException("At least one valid role must be assigned to the user.");

                var companiesRoles = companiesResult.Select(c => new LaqApiUserCompanies(createdUser.Id, c)).ToList();

                if (companiesRoles.Count > 0)
                    await _userRepository.AddUserCompanies(companiesRoles);

                var userIntegrations = integrationsResult.Select(i => new LaqApiUserIntegrations(createdUser.Id, i)).ToList();
                if (userIntegrations.Count > 0)
                    await _userRepository.AddUserIntegrations(userIntegrations);

                await _unitOfWork.CommitAsync();

                return new UserResponseDTO(createdUser.Id
                                     , createdUser.Username
                                     , createdUser.Status?.Description ?? null
                                     , null
                                     , null
                                     , null);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }
        }

        public async Task<(List<UserResponseDTO> Data, int DataCount)> GetUsers(int page, int pageSize, string orderBy, bool ascending, UserFilters? filters)
        {
            var (entities, count) = await _userRepository.GetUsers(page, pageSize, orderBy, ascending, filters = new UserFilters());

            return (entities.Select(entity => new UserResponseDTO(
                entity.Id,
                entity.Username,
                entity.Status?.Description ?? null,
                entity.UserCompanies?.ToDictionary(uc => uc.CompanyId, uc => uc.Company?.CompanyName ?? string.Empty),
                entity.UserIntegrations?.ToDictionary(ui => ui.IntegrationId, ui => ui.Integration?.IntegrationName ?? string.Empty),
                entity.UserRoles?.ToDictionary(ur => ur.RoleId, ur => ur.Role?.RoleName ?? string.Empty)
            )).ToList(), count);
        }

        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            var entity = await _userRepository.GetUserById(id);
            return new UserResponseDTO(entity.Id,
                entity.Username,
                entity.Status?.Description ?? null,
                entity.UserCompanies?.ToDictionary(uc => uc.CompanyId, uc => uc.Company?.CompanyName ?? string.Empty),
                entity.UserIntegrations?.ToDictionary(ui => ui.IntegrationId, ui => ui.Integration?.IntegrationName ?? string.Empty),
                entity.UserRoles?.ToDictionary(ur => ur.RoleId, ur => ur.Role?.RoleName ?? string.Empty));
        }

        public async Task<UserResponseDTO> UpdateUser(Guid id, UserDTO dto)
        {
            if (!await _userRepository.UserExistsAsync(id))
                throw new NotFoundException("No user found with the given id.");

            var actualUser = await _userRepository.GetUserById(id);

            var actualRoles = actualUser.UserRoles?.ToList() ?? new();
            var actualCompanies = actualUser.UserCompanies?.ToList() ?? new();
            var actualIntegrations = actualUser.UserIntegrations?.ToList() ?? new();

            List<Guid> existingCompanies = await _companyRepository.GetAllCompanyIds();
            List<Guid> existingIntegrations = await _integrationRepository.GetAllIntegrationIds();
            var existRoles = await _userRepository.GetRolesAsync();

            List<int> existingRoles = existRoles.Select(x => x.RoleId).ToList();

            if (dto.Companies != null && dto.Companies.Count() > 0)
                dto.Companies = dto.Companies.Where(x => existingCompanies.Contains(x)).ToList();

            if (dto.Integrations != null && dto.Integrations.Count() > 0)
                dto.Integrations = dto.Integrations.Where(x => existingIntegrations.Contains(x)).ToList();

            dto.Roles = dto.Roles.Where(x => existingRoles.Contains(x)).ToList();

            // ----- ROLES -----
            var newRoles = dto.Roles?.Distinct().Select(r => new LaqApiUserRoles(id, r)).ToList() ?? new();
            var rolesToAdd = newRoles.Where(nr => !actualRoles.Any(ar => ar.RoleId == nr.RoleId)).ToList();
            var rolesToRemove = actualRoles.Where(ar => !newRoles.Any(nr => nr.RoleId == ar.RoleId)).ToList();

            if (!newRoles.Any())
                throw new BadRequestException("At least one valid role must be assigned to the user.");


            // ----- COMPANIES -----
            var newCompanies = dto.Companies?.Distinct().Select(c => new LaqApiUserCompanies(id, c)).ToList() ?? new();
            var companiesToAdd = newCompanies.Where(nc => !actualCompanies.Any(ac => ac.CompanyId == nc.CompanyId)).ToList();
            var companiesToRemove = actualCompanies.Where(ac => !newCompanies.Any(nc => nc.CompanyId == ac.CompanyId)).ToList();


            // ----- INTEGRATIONS -----
            var newIntegrations = dto.Integrations?.Distinct().Select(i => new LaqApiUserIntegrations(id, i)).ToList() ?? new();
            var integrationsToAdd = newIntegrations.Where(ni => !actualIntegrations.Any(ai => ai.IntegrationId == ni.IntegrationId)).ToList();
            var integrationsToRemove = actualIntegrations.Where(ai => !newIntegrations.Any(ni => ni.IntegrationId == ai.IntegrationId)).ToList();


            var user = new LaqApiUsers(
                dto.Username,
                actualUser.Hash,
                actualUser.Salt,
                dto.StatusId ?? actualUser.StatusId
            )
            {
                Id = id,
                ModifiedAt = DateTime.UtcNow
            };
            
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (rolesToAdd.Any())
                    await _userRepository.AddUserRoles(rolesToAdd);

                if (companiesToAdd.Any())
                    await _userRepository.AddUserCompanies(companiesToAdd);

                if (integrationsToAdd.Any())
                    await _userRepository.AddUserIntegrations(integrationsToAdd);

                if (rolesToRemove.Any() || companiesToRemove.Any() || integrationsToRemove.Any())
                    await _userRepository.RemoveJoinedUserTables(rolesToRemove, companiesToRemove, integrationsToRemove);

                var updatedUser = await _userRepository.UpdateUser(user);

                return new UserResponseDTO(
                            updatedUser.Id,
                            updatedUser.Username,
                            updatedUser.Status?.Description ?? null,
                            updatedUser.UserCompanies?.ToDictionary(uc => uc.CompanyId, uc => uc.Company?.CompanyName ?? string.Empty),
                            updatedUser.UserIntegrations?.ToDictionary(ui => ui.IntegrationId, ui => ui.Integration?.IntegrationName ?? string.Empty),
                            updatedUser.UserRoles?.ToDictionary(ur => ur.RoleId, ur => ur.Role?.RoleName ?? string.Empty)
                        );
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }
        }

    }
}