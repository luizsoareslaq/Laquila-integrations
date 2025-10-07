using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Domain.DTO.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/outbound")]
    public class OutboundController : ControllerBase
    {
        private readonly IPrenotaService _prenotaService;
        private readonly IExternalService _externalService;
        protected readonly ErrorCollector errors = new ErrorCollector();


        public OutboundController(IPrenotaService prenotaService, IExternalService externalService)
        {
            _prenotaService = prenotaService;
            _externalService = externalService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] LAQFilters filters)
        {
            var prenotas = await _prenotaService.GetPrenotasAsync(filters, CancellationToken.None);

            return Ok(prenotas);
        }

        //1.1.1
        [HttpPost("orders/external/{apiIntegrationId}")]
        public async Task<IActionResult> SendOrderAsync([FromBody] PrenotaDTO dto, Guid apiIntegrationId)
        {
            var response = await _externalService.SendOrdersAsync(dto, apiIntegrationId);

            return Ok(response);
        }

        //1.1.2
        [Authorize]
        [HttpPatch("orders/{lo_oe}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromRoute] long lo_oe, [FromBody] PrenotaDatesDTO dto)
        {
            if (dto.LoDtIniPicking == null && dto.LoDtEndPicking == null && dto.LoDtIniConf == null && dto.LoDtEndConf == null)
                errors.Add("", "", "", "At least one date must be provided.", true);

            var response = await _prenotaService.UpdatePrenotasStatusAsync(lo_oe, dto);

            return Ok(response);
        }

        //1.1.3
        [HttpPut("orders/{lo_oe}")]
        public async Task<IActionResult> UpdateRenouncedItemsAsync([FromRoute] long lo_oe, [FromBody] PrenotaRenouncedDTO dto)
        {
            if(dto.Items == null || !dto.Items.Any())
                errors.Add("", "", "", "At least one item must be provided.", true);

            var response = await _prenotaService.UpdateRenouncedItemsAsync(lo_oe, dto);

            return Ok(response);
        }

        //1.2.1
        [HttpPost("invoice/{lo_oe}")]
        public async Task<IActionResult> SendInvoiceAsync([FromRoute] long lo_oe, [FromBody] InvoiceDTO dto)
        {
            return Ok();
        }

        //1.2.2
        [HttpPatch("invoice/{lo_oe}/status")]
        public async Task<IActionResult> UpdateInvoiceStatusAsync([FromRoute] long lo_oe, [FromBody] InvoiceDatesDTO dto)
        {
            return Ok();
        }
    }
}