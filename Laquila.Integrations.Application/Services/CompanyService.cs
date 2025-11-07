using Laquila.Integrations.Application.DTO.Company.Request;
using Laquila.Integrations.Application.DTO.Company.Response;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Filters;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(ICompanyRepository companyRepository
                            , IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyResponseDTO> CreateCompany(CompanyDTO dto)
        {
            if (await _companyRepository.CompanyDocumentExists(dto.Document))
                throw new BadRequestException(MessageProvider.Get("CompanyDocumentAlreadyExists", UserContext.Language));

            dto.StatusId = 1; // Default to Active

            var company = new LaqApiCompany(dto.ErpCode, dto.CompanyName, dto.Document, dto.StatusId);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var createdCompany = await _companyRepository.CreateCompany(company);

                await _unitOfWork.CommitAsync();

                return new CompanyResponseDTO(createdCompany.Id, createdCompany.ErpCode, createdCompany.CompanyName, createdCompany.Document, null);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }
        }

        public async Task<(List<CompanyResponseDTO> Data, int DataCount)> GetCompanies(int page, int pageSize, string orderBy, bool ascending, CompanyFilters? filters)
        {
            var (entities, count) = await _companyRepository.GetCompanies(page, pageSize, orderBy, ascending, filters = new CompanyFilters());

            return (entities.Select(entity => new CompanyResponseDTO(
                entity.Id,
                entity.ErpCode,
                entity.CompanyName,
                entity.Document,
                entity.Status?.Description ?? null
            )).ToList(), count);
        }

        public async Task<CompanyResponseDTO> GetCompanyById(Guid id)
        {
            var company = await _companyRepository.GetCompanyById(id);
            return new CompanyResponseDTO(company.Id, company.ErpCode, company.CompanyName, company.Document, company.Status?.Description ?? null);
        }

        public async Task<CompanyResponseDTO> UpdateCompany(Guid id, CompanyDTO dto)
        {
            if (!await _companyRepository.CompanyIdExists(id))
                throw new NotFoundException(MessageProvider.Get("CompanyIdNotFound", UserContext.Language));

            if (await _companyRepository.CompanyDocumentExistsWithId(dto.Document, id))
                throw new BadRequestException(MessageProvider.Get("CompanyDocumentAlreadyExists", UserContext.Language));

            var company = new LaqApiCompany(dto.ErpCode, dto.CompanyName, dto.Document, dto.StatusId);
            company.Id = id;
            company.ModifiedAt = DateTime.Now;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var updatedCompany = await _companyRepository.UpdateCompany(company);

                await _unitOfWork.CommitAsync();

                return new CompanyResponseDTO(updatedCompany.Id, updatedCompany.ErpCode, updatedCompany.CompanyName, updatedCompany.Document, updatedCompany.Status?.Description ?? null);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message + " - " + ex.InnerException?.Message);
            }


        }
    }
}