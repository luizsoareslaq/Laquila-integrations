using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.Repositories.LaqHub
{
    public class UserRepository : IUserRepository
    {
        private readonly LaquilaHubContext _context;
        private readonly string lang = UserContext.Language ?? "en";

        public UserRepository(LaquilaHubContext laqHubContext)
        {
            _context = laqHubContext;
        }
        public async Task<LaqApiUsers> CreateUserAsync(LaqApiUsers user)
        {
            _context.LaqApiUsers.Add(user);
            await _context.SaveChangesAsync();

            return await _context.LaqApiUsers
                .Include(u => u.Status)
                .FirstOrDefaultAsync(u => u.Id == user.Id) ?? throw new EntityNotFoundAfterCreated("User"); ;
        }

        public async Task AddUserRoles(List<LaqApiUserRoles> userRoles)
        {
            _context.LaqApiUserRoles.AddRange(userRoles);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.LaqApiUsers.AnyAsync(x => x.Username == username);
        }

        public async Task<List<LaqApiRoles>> GetRolesAsync()
        {
            var roles = await _context.LaqApiRoles.ToListAsync();
            return roles ?? throw new NotFoundException(MessageProvider.Get("RolesDatabaseNotFound", lang));
        }

        public async Task AddUserCompanies(List<LaqApiUserCompanies> userCompanies)
        {
            _context.LaqApiUserCompanies.AddRange(userCompanies);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserIntegrations(List<LaqApiUserIntegrations> userIntegrations)
        {
            _context.LaqApiUserIntegrations.AddRange(userIntegrations);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<LaqApiUsers> Data, int DataCount)> GetUsers(int page, int pageSize, string orderBy, bool ascending, UserFilters filters)
        {
            var query = _context.LaqApiUsers.AsQueryable();

            if (filters != null)
            {
                if (filters.Id.HasValue)
                    query = query.Where(x => x.Id == filters.Id.Value);

                if (!string.IsNullOrEmpty(filters.Username))
                    query = query.Where(x => x.Username == filters.Username);

                if (filters.Companies != null && filters.Companies.Any())
                    query = query.Include(u => u.UserCompanies)
                                 .Where(u => u.UserCompanies.Any(uc => filters.Companies.Contains(uc.CompanyId)));

                if (filters.Integrations != null && filters.Integrations.Any())
                    query = query.Include(u => u.UserIntegrations)
                                 .Where(u => u.UserIntegrations.Any(ui => filters.Integrations.Contains(ui.IntegrationId)));

                if (filters.Roles != null && filters.Roles.Any())
                    query = query.Include(u => u.UserRoles)
                                 .Where(u => u.UserRoles.Any(ur => filters.Roles.Contains(ur.RoleId)));

                if (filters.StatusId.HasValue)
                    query = query.Where(x => x.StatusId == filters.StatusId.Value);
            }

            query = orderBy.ToLower() switch
            {
                "username" => ascending ? query.OrderBy(x => x.Username) : query.OrderByDescending(x => x.Username),
                "status" => ascending ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.StatusId),
                "createdat" => ascending ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                _ => ascending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
            };

            var totalItems = await query.CountAsync();

            var items = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.UserRoles).ThenInclude(ur => ur.Role)
                            .Include(x => x.UserCompanies).ThenInclude(uc => uc.Company)
                            .Include(x => x.UserIntegrations).ThenInclude(ui => ui.Integration)
                            .Include(x => x.Status)
                            .ToListAsync() ?? throw new NotFoundException(MessageProvider.Get("UsersNotFound", lang));

            return (items, totalItems);
        }

        public async Task<LaqApiUsers> GetUserById(Guid id)
        {
            return await _context.LaqApiUsers
                .Include(x => x.UserRoles).ThenInclude(ur => ur.Role)
                .Include(x => x.UserCompanies).ThenInclude(uc => uc.Company)
                .Include(x => x.UserIntegrations).ThenInclude(ui => ui.Integration)
                .Include(x => x.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException(MessageProvider.Get("UserIdNotFound", lang));
        }

        public async Task<LaqApiUsers> UpdateUser(LaqApiUsers entity)
        {
            _context.LaqApiUsers.Update(entity);
            await _context.SaveChangesAsync();

            return await _context.LaqApiUsers
                .Include(x => x.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(x => x.UserCompanies)
                    .ThenInclude(uc => uc.Company)
                .Include(x => x.UserIntegrations)
                    .ThenInclude(ui => ui.Integration)
                .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? throw new EntityNotFoundAfterUpdated("User");
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await _context.LaqApiUsers.AnyAsync(x => x.Id == id);
        }

        public async Task RemoveJoinedUserTables(List<LaqApiUserRoles> userRoles, List<LaqApiUserCompanies> userCompanies, List<LaqApiUserIntegrations> userIntegrations)
        {
            if (userRoles.Any())
                _context.LaqApiUserRoles.RemoveRange(userRoles);
            if (userCompanies.Any())
                _context.LaqApiUserCompanies.RemoveRange(userCompanies);
            if (userIntegrations.Any())
                _context.LaqApiUserIntegrations.RemoveRange(userIntegrations);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Guid>> GetAllUserIds()
        {
            return await _context.LaqApiUsers.Select(x => x.Id).ToListAsync();
        }

        public async Task<LaqApiUsers> GetUserByUsername(string username)
        {
            var user = await _context.LaqApiUsers
                            .Include(x => x.UserRoles)
                                .ThenInclude(x => x.Role)
                            .Include(x => x.UserIntegrations)
                                .ThenInclude(x => x.Integration)
                            .Include(x => x.UserCompanies)
                                .ThenInclude(x => x.Company)
                            .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                throw new NotFoundException(string.Format(MessageProvider.Get("UsernameNotFound", lang), username));

            return user;
        }
    }
}