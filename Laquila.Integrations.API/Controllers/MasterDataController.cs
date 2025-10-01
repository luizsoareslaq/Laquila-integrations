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
        //3.1.1
        [HttpPost("items")]
        public async Task<IActionResult> SendItems([FromBody] MasterDataItemsDTO dto)
        {
            return Ok();
        }

        //Adicionar método de GET para os itens também

        //3.1.2
        [HttpPost("items/packages")]
        public async Task<IActionResult> SendItemPackages([FromBody] MasterDataItemPackagesDTO dto)
        {
            return Ok();
        }

        //3.1.3
        [HttpPost("mandators")]
        public async Task<IActionResult> SendMandators([FromBody] MasterDataMandatorsDTO dto)
        {
            return Ok();
        }
    }
}