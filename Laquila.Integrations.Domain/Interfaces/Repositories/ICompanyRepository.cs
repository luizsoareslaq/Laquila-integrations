using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<LaqApiCompany> CreateCompany(LaqApiCompany entity);
        Task<(List<LaqApiCompany> Data, int DataCount)> GetCompanies(int page, int pageSize, string orderBy, bool ascending, CompanyFilters filters);
        Task<LaqApiCompany> GetCompanyById(Guid id);
        Task<List<Guid>> GetAllCompanyIds();
        Task<LaqApiCompany> UpdateCompany(LaqApiCompany entity);

        Task<bool> CompanyDocumentExists(string integrationName);
        Task<bool> CompanyIdExists(Guid id);
        Task<bool> CompanyDocumentExistsWithId(string document, Guid id);
    }
}