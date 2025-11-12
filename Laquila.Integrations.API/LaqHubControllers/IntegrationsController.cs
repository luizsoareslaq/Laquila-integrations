using Laquila.Integrations.Application.DTO.ApiIntegration.Request;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.LaqHubControllers
{
    [ApiController]
    [Route("api/integrations")]
    public class IntegrationsController : ControllerBase
    {
        private readonly IApiIntegrationsService _integrationsService;
        public IntegrationsController(IApiIntegrationsService integrationsService)
        {
            _integrationsService = integrationsService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody, Bind("IntegrationName,StatusId")] ApiIntegrationDTO dto)
        {
            var apiIntegration = await _integrationsService.CreateApiIntegration(dto);

            return Created("Created", apiIntegration);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string orderBy = "id", bool ascending = true)
        {
            (var apiIntegrations, int count) = await _integrationsService.GetApiIntegrations(page, pageSize, orderBy, ascending);

            return Ok(new { Data = apiIntegrations, Count = count });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var apiIntegration = await _integrationsService.GetApiIntegrationById(id);

            return Ok(apiIntegration);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ApiIntegrationDTO dto)
        {
            await _integrationsService.UpdateApiIntegration(id, dto);

            return NoContent();
        }
    }
}