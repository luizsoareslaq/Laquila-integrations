using Laquila.Integrations.Application.DTO.Company.Request;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CompanyDTO dto)
        {
            var company = await _companyService.CreateCompany(dto);

            return Created($"\"{dto.CompanyName}\" created.", company);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string orderBy = "id", bool ascending = true, [FromQuery] CompanyFilters? filters = null)
        {
            (var companies, int count) = await _companyService.GetCompanies(page, pageSize, orderBy, ascending, filters);

            return Ok(new { Data = companies, Count = count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var company = await _companyService.GetCompanyById(id);

            return Ok(company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CompanyDTO dto)
        {
            await _companyService.UpdateCompany(id, dto);

            return NoContent();
        }
    }
}