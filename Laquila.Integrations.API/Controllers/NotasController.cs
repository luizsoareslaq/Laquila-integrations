using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/notas")]
    [Authorize]
    public class NotasController : ControllerBase
    {
        private readonly INotaService _notaService;

        public NotasController(INotaService notaService)
        {
            _notaService = notaService;
        }

        [HttpGet]
        public async Task<ActionResult> GetNotasAsync([FromQuery] LAQFilters filters, CancellationToken ct)
        {
            var response = await _notaService.GetNotasAsync(filters, ct);

            return Ok(response);
        }
    }
}