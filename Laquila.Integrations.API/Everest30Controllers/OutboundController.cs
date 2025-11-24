using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.Everest30Controllers
{
    [ApiController]
    [Route("api/outbound")]
    public class OutboundController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IExternalService _externalService;
        protected readonly ErrorCollector errors = new ErrorCollector();

        private readonly string lang = UserContext.Language ?? "en";

        public OutboundController(IOrdersService ordersService, IExternalService externalService)
        {
            _ordersService = ordersService;
            _externalService = externalService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] LAQFilters filters)
        {
            var prenotas = await _ordersService.GetUnsentOrdersAsync(filters, CancellationToken.None);

            return Ok(prenotas);
        }

        //1.1.1
        [Authorize(Roles="Admin")]
        [HttpPost("orders/{integrationId}")]
        public async Task<IActionResult> SendOrderAsync([FromBody] PrenotaDTO dto, Guid integrationId)
        {
            var response = await _externalService.SendOrdersAsync(dto, integrationId);

            return Ok(response);
        }

        //1.1.2
        [Authorize(Roles ="Admin,Read-Write,Read-Only")]
        [HttpPatch("orders/{lo_oe}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromRoute] long lo_oe, [FromBody] PrenotaDatesDTO dto)
        {
            if (dto.LoDtIniPicking == null && dto.LoDtEndPicking == null && dto.LoDtIniConf == null && dto.LoDtEndConf == null)
                errors.Add("", "", "", MessageProvider.Get("AtLeastOneDateRequired",lang), true);

            var response = await _ordersService.UpdateOrderStatusAsync(lo_oe, dto);

            return Ok(response);
        }

        //1.1.3
        [Authorize(Roles ="Admin,Read-Write,Read-Only")]
        [HttpPut("orders/{lo_oe}")]
        public async Task<IActionResult> UpdateRenouncedItemsAsync([FromRoute] long lo_oe, [FromBody] PrenotaRenouncedDTO dto)
        {
            if(dto.Items == null || !dto.Items.Any())
                errors.Add("", "", "", MessageProvider.Get("AtLeastOneItemRequired",lang), true);

            var response = await _ordersService.UpdateRenouncedItemsAsync(lo_oe, dto);

            return Ok(response);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("invoice")]
        public async Task<IActionResult> GetInvoiceOrdersAsync([FromQuery] LAQFilters filters)
        {
            var prenotas = await _ordersService.GetUnsentInvoicesAsync(filters, CancellationToken.None);

            return Ok(prenotas);
        }

        //1.2.1
        [Authorize(Roles = "Admin")]
        [HttpPost("invoice/{integrationId}")]
        public async Task<IActionResult> SendInvoiceAsync([FromBody] InvoiceDTO dto, Guid integrationId)
        {
            var response = await _externalService.SendInvoicesAsync(dto, integrationId);

            return Ok(response);
        }

        //1.2.2
        [Authorize(Roles ="Admin,Read-Write,Read-Only")]
        [HttpPatch("invoice/{lo_oe}/status")]
        public async Task<IActionResult> UpdateInvoiceStatusAsync([FromRoute] long lo_oe, [FromBody] InvoiceDatesDTO dto)
        {
            return Ok();
        }
    }
}