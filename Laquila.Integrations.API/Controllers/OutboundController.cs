using Laquila.Integrations.Core.Domain.DTO.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/outbound")]
    public class OutboundController : ControllerBase
    {
        //1.1.1
        [HttpPost("orders/{lo_oe}")]
        public async Task<IActionResult> SendOrderAsync([FromQuery] long lo_oe)
        {
            return Ok();
        }

        //1.1.2
        [HttpPatch("orders/{lo_oe}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromRoute] long lo_oe, [FromBody] PrenotaDatesDTO dto)
        {
            return Ok();
        }

        //1.1.3
        [HttpPut("orders/{lo_oe}")]
        public async Task<IActionResult> UpdateRenouncedItemsAsync([FromRoute] long lo_oe, [FromBody] PrenotaRenouncedDTO dto)
        {
            return Ok();
        }

        //1.2.1
        [HttpPost("invoice/{lo_oe}")]
        public async Task<IActionResult> SendInvoiceAsync([FromRoute] long lo_oe)
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