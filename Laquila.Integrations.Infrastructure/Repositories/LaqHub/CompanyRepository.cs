using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.Repositories.LaqHub
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly LaquilaHubContext _context;
        public CompanyRepository(LaquilaHubContext context)
        {
            _context = context;
        }
        public async Task<bool> CompanyDocumentExists(string document)
        {
            return await _context.LaqApiCompanies.AnyAsync(x => x.Document == document);
        }

        public async Task<bool> CompanyDocumentExistsWithId(string document, Guid id)
        {
            return await _context.LaqApiCompanies.AnyAsync(x => x.Document == document && x.Id != id);
        }

        public async Task<LaqApiCompany> CreateCompany(LaqApiCompany entity)
        {
            _context.LaqApiCompanies.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Guid>> GetAllCompanyIds()
        {
            return await _context.LaqApiCompanies.Select(x => x.Id).ToListAsync();
        }

        public async Task<(List<LaqApiCompany> Data, int DataCount)> GetCompanies(int page, int pageSize, string orderBy, bool ascending, CompanyFilters filters)
        {
            var query = _context.LaqApiCompanies.AsQueryable();

            if (filters != null)
            {
                if (filters.Id.HasValue)
                    query = query.Where(x => x.Id == filters.Id.Value);

                if (filters.ErpCode.HasValue)
                    query = query.Where(x => x.ErpCode == filters.ErpCode.Value);

                if (!string.IsNullOrEmpty(filters.Document))
                    query = query.Where(x => x.Document == filters.Document);

                if (filters.StatusId.HasValue)
                    query = query.Where(x => x.StatusId == filters.StatusId.Value);
            }

            query = orderBy.ToLower() switch
            {
                "companyname" => ascending ? query.OrderBy(x => x.CompanyName) : query.OrderByDescending(x => x.CompanyName),
                "document" => ascending ? query.OrderBy(x => x.Document) : query.OrderByDescending(x => x.Document),
                "createdat" => ascending ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                _ => ascending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
            };

            var totalItems = await query.CountAsync();

            var items = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.Status)
                            .ToListAsync() ?? throw new NotFoundException(MessageProvider.Get("CompaniesNotFound", UserContext.Language));

            return (items, totalItems);
        }

        public async Task<LaqApiCompany> GetCompanyById(Guid id)
        {
            return await _context.LaqApiCompanies.Where(x => x.Id == id)
                                                .Include(x => x.Status)
                                                .FirstOrDefaultAsync() ?? throw new NotFoundException(MessageProvider.Get("CompanyIdNotFound", UserContext.Language));
        }

        public async Task<bool> CompanyIdExists(Guid id)
        {
            return await _context.LaqApiCompanies.AnyAsync(x => x.Id == id);
        }

        public async Task<LaqApiCompany> UpdateCompany(LaqApiCompany entity)
        {
            _context.LaqApiCompanies.Update(entity);
            await _context.SaveChangesAsync();

            return await _context.LaqApiCompanies
                .Include(u => u.Status)
                .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? throw new EntityNotFoundAfterUpdated("Company");
        }
    }
}