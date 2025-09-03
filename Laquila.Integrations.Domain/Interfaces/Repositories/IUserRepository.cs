using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<LaqApiUsers> CreateUserAsync(LaqApiUsers user);
        Task<(List<LaqApiUsers> Data, int DataCount)> GetUsers(int page, int pageSize, string orderBy, bool ascending, UserFilters filters);
        Task<LaqApiUsers> GetUserById(Guid id);
        Task<LaqApiUsers> GetUserByUsername(string id);
        Task<LaqApiUsers> UpdateUser(LaqApiUsers entity);

        Task<bool> UsernameExistsAsync(string username);
        Task<bool> UserExistsAsync(Guid id);
        Task<List<LaqApiRoles>> GetRolesAsync();
        Task<List<Guid>> GetAllUserIds();

        //Join tables
        Task AddUserRoles(List<LaqApiUserRoles> userRoles);
        Task AddUserCompanies(List<LaqApiUserCompanies> userCompanies);
        Task AddUserIntegrations(List<LaqApiUserIntegrations> userIntegrations);

        Task RemoveJoinedUserTables(List<LaqApiUserRoles> userRoles, List<LaqApiUserCompanies> userCompanies, List<LaqApiUserIntegrations> userIntegrations);
    }
}