using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/romaneio")]
    public class RomaneioController : ControllerBase
    {
        private readonly IRomaneioService _romaneioService;
        public RomaneioController(IRomaneioService romaneioService)
        {
            _romaneioService = romaneioService;
        }
        [HttpGet]
        public async Task<ActionResult> GetRomaneioAsync([FromQuery] LAQFilters filters, CancellationToken ct)
        {
            var response = await _romaneioService.GetRomaneiosAsync(filters, ct);

            return Ok(response);
        }
    }
}