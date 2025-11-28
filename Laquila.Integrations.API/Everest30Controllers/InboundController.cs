using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.Everest30Controllers
{
    [ApiController]
    [Route("api/inbound")]
    public class InboundController : ControllerBase
    {
        private readonly IInboundService _inboundService;
        private readonly IExternalService _externalService;
        protected readonly ErrorCollector errors = new ErrorCollector();

        private readonly string lang = UserContext.Language ?? "en";

        public InboundController(IExternalService externalService, IInboundService inboundService)
        {
            _inboundService = inboundService;
            _externalService = externalService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("orders")]
        public async Task<IActionResult> GetReceiveInvoicesAsync([FromQuery] LAQFilters filters)
        {
            var prenotas = await _inboundService.GetUnsentReceiveInvoicesAsync(filters, CancellationToken.None);

            return Ok(prenotas);
        }

        //2.1.1
        [HttpPost("invoice/{integrationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendInvoiceAsync([FromBody] ReceiveInvoiceDTO dto, Guid integrationId)
        {
            var response = await _externalService.SendReceiveInvoicesAsync(dto, integrationId);

            return Ok(response);
        }

        //2.1.2
        [HttpPatch("invoice/{li_id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInvoiceStatusAsync([FromRoute] long li_id, [FromBody] ReceiveInvoiceDatesDTO dto)
        {
            return Ok();
        }

        //2.1.3
        [HttpPatch("invoice/{li_id}/items/receiving")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInvoiceItemStatusAsync([FromRoute] long li_id, [FromBody] InvoiceItemStatusDTO dto)
        {
            return Ok();
        }

        //2.1.4
        [HttpPut("invoice/{li_id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInvoiceQualityAssuranceAsync([FromRoute] long li_id, [FromBody] InvoiceQualityAssuranceDTO dto)
        {
            return Ok();
        }
    }
}