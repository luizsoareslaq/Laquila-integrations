using Laquila.Integrations.Application.DTO.ApiIntegration.Request;
using Laquila.Integrations.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/integrations")]
    public class IntegrationsController : ControllerBase
    {
        private readonly IApiIntegrationsService _integrationsService;
        public IntegrationsController(IApiIntegrationsService integrationsService)
        {
            _integrationsService = integrationsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody, Bind("IntegrationName,StatusId")] ApiIntegrationDTO dto)
        {
            var apiIntegration = await _integrationsService.CreateApiIntegration(dto);

            return Created("Integration created successfully.", apiIntegration);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string orderBy = "id", bool ascending = true)
        {
            (var apiIntegrations, int count) = await _integrationsService.GetApiIntegrations(page, pageSize, orderBy, ascending);

            return Ok(new { Data = apiIntegrations, Count = count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var apiIntegration = await _integrationsService.GetApiIntegrationById(id);

            return Ok(apiIntegration);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ApiIntegrationDTO dto)
        {
            await _integrationsService.UpdateApiIntegration(id, dto);

            return NoContent();
        }
    }
}