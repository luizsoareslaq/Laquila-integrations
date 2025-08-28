using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class ApiIntegrationsRepository : IApiIntegrationsRepository
    {
        private readonly LaquilaHubContext _context;
        public ApiIntegrationsRepository(LaquilaHubContext context)
        {
            _context = context;
        }

        public async Task AddIntegrationCompanies(List<LaqApiIntegrationCompanies> integrationCompanies)
        {
            _context.LaqApiIntegrationCompanies.AddRange(integrationCompanies);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserIntegrations(List<LaqApiUserIntegrations> userIntegrations)
        {
            _context.LaqApiUserIntegrations.AddRange(userIntegrations);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ApiIntegrationsExistsAsync(string integrationName, Guid id)
        {
            return await _context.LaqApiIntegrations.AnyAsync(x => x.IntegrationName == integrationName && (id != Guid.Empty || x.Id != id));
        }

        public async Task<LaqApiIntegrations> CreateApiIntegration(LaqApiIntegrations apiIntegration)
        {
            _context.LaqApiIntegrations.Add(apiIntegration);
            await _context.SaveChangesAsync();

            return await _context.LaqApiIntegrations
                .Include(u => u.Status)
                .FirstOrDefaultAsync(u => u.Id == apiIntegration.Id) ?? throw new EntityNotFoundAfterCreated("API Integration");
        }
        
        public async Task<List<Guid>> GetAllIntegrationIds()
        {
            return await _context.LaqApiIntegrations.Select(x => x.Id).ToListAsync();
        }

        public async Task<LaqApiIntegrations> GetApiIntegrationById(Guid id)
        {
            return await _context.LaqApiIntegrations.Where(x => x.Id == id)
            .Include(x => x.Status)
            .Include(x => x.IntegrationCompanies).ThenInclude(ic => ic.Company)
            .Include(x => x.UserIntegrations).ThenInclude(ic => ic.User)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("No integration found with the given ID.");
        }

        public async Task<(List<LaqApiIntegrations> Data, int DataCount)> GetApiIntegrations(int page, int pageSize, string orderBy, bool ascending)
        {
            var query = _context.LaqApiIntegrations.AsQueryable();

            query = orderBy.ToLower() switch
            {
                "integrationname" => ascending ? query.OrderBy(x => x.IntegrationName) : query.OrderByDescending(x => x.IntegrationName),
                "createdat" => ascending ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                _ => ascending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
            };

            var totalItems = await query.CountAsync();

            var items = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.Status)
                            .Include(x => x.IntegrationCompanies).ThenInclude(ic => ic.Company)
                            .Include(x => x.UserIntegrations).ThenInclude(ic => ic.User)
                            .ToListAsync() ?? throw new NotFoundException("No integrations found with these filters.");

            return (items, totalItems);
        }

        public Task RemoveJoinedIntegrationTables(List<LaqApiUserIntegrations> userIntegrations, List<LaqApiIntegrationCompanies> integrationCompanies)
        {
            if (userIntegrations.Any())
                _context.LaqApiUserIntegrations.RemoveRange(userIntegrations);
            if (integrationCompanies.Any())
                _context.LaqApiIntegrationCompanies.RemoveRange(integrationCompanies);
            return _context.SaveChangesAsync();
        }

        public async Task<LaqApiIntegrations> UpdateApiIntegration(LaqApiIntegrations apiIntegration)
        {
            _context.LaqApiIntegrations.Update(apiIntegration);
            await _context.SaveChangesAsync();

            return await _context.LaqApiIntegrations
                .Include(u => u.Status)
                .FirstOrDefaultAsync(u => u.Id == apiIntegration.Id) ?? throw new EntityNotFoundAfterUpdated("API Integration");
        }
    }
}