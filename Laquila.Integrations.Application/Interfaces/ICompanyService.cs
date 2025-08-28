using Laquila.Integrations.Application.DTO.Company.Request;
using Laquila.Integrations.Application.DTO.Company.Response;
using Laquila.Integrations.Domain.Filters;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDTO> CreateCompany(CompanyDTO dto);
        Task<(List<CompanyResponseDTO> Data, int DataCount)> GetCompanies(int page, int pageSize, string orderBy, bool ascending, CompanyFilters? filters);
        Task<CompanyResponseDTO> GetCompanyById(Guid id);
        Task<CompanyResponseDTO> UpdateCompany(Guid id, CompanyDTO dto);
    }
}