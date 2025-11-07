using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.Everest30Controllers
{
    [ApiController]
    [Route("api/masterdata")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /* ITENS */
        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var result = await _masterDataService.GetUnsentItemsAsync(new LAQFilters { PageSize = 100 }, CancellationToken.None);

            return Ok(result);
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
            var result = await _masterDataService.GetUnsentMandatorAsync(new LAQFilters { PageSize = 100 }, CancellationToken.None);

            return Ok(result);
        }

        [HttpPost("mandators")]
        public async Task<IActionResult> SendMandators([FromBody] MasterDataMandatorsDTO dto)
        {
            return Ok();
        }

    }
}