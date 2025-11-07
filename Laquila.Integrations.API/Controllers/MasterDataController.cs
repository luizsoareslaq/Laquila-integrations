using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Route("api/masterdata")]
    public class MasterDataController : ControllerBase
    {
        /* ITENS */

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            return Ok();
        }

        //3.1.1
        [HttpPost("items")]
        public async Task<IActionResult> SendItems([FromBody] MasterDataItemsPackageDTO dto)
        {
            return Ok();
        }

        /* CLIENTES */

        //3.1.2
        [HttpGet("mandators")]
        public async Task<IActionResult> GetMandators()
        {
            return Ok();
        }

        [HttpPost("mandators")]
        public async Task<IActionResult> SendMandators([FromBody] MasterDataMandatorsDTO dto)
        {
            return Ok();
        }

    }
}