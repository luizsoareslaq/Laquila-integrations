using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/inbound")]
    public class InboundController : ControllerBase
    {
        //2.1.1
        [HttpPost("invoice/{li_id}")]
        public async Task<IActionResult> SendInvoiceAsync([FromRoute] long li_id)
        {
            return Ok();
        }

        //2.1.2
        [HttpPatch("invoice/{li_id}/status")]
        public async Task<IActionResult> UpdateInvoiceStatusAsync([FromRoute] long li_id, [FromBody] ReceiveInvoiceDatesDTO dto)
        {
            return Ok();
        }

        //2.1.3
        [HttpPut("invoice/{li_id}")]
        public async Task<IActionResult> UpdateInvoiceQualityAssurance([FromRoute] long li_id, [FromBody] InvoiceQualityAssuranceDTO dto)
        {
            return Ok();
        }
    }
}