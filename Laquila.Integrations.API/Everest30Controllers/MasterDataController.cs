using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers.Everest30Controllers
{
    [ApiController]
    [Route("api/masterdata")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;
        private readonly IExternalService _externalService;

        public MasterDataController(IMasterDataService masterDataService
                                  , IExternalService externalService)
        {
            _masterDataService = masterDataService;
            _externalService = externalService;
        }

        /*************************************************************************************************************************************/
        /**************************************************************** ITENS **************************************************************/
        /*************************************************************************************************************************************/

        [Authorize(Roles="Admin")]
        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] MasterDataFilters filters)
        {
            var result = await _masterDataService.GetUnsentItemsAsync(filters, CancellationToken.None);

            return Ok(result);
        }

        //3.1.1
        [Authorize(Roles="Admin")]
        [HttpPost("items/{integrationId}")]
        public async Task<IActionResult> SendItems([FromBody] MasterDataItemsPackageDTO dto, Guid integrationId)
        {
            // var result = await _externalService.SendItemsAsync(dto, integrationId);

            var response = await _masterDataService.HandleItemsAsync(dto);

            return Ok(response);
        }

        /*************************************************************************************************************************************/
        /************************************************************** CLIENTES *************************************************************/
        /*************************************************************************************************************************************/

        //3.1.2
        [Authorize(Roles="Admin")]
        [HttpGet("mandators")]
        public async Task<IActionResult> GetMandators([FromQuery] MasterDataFilters filters)
        {
            var result = await _masterDataService.GetUnsentMandatorAsync(filters, CancellationToken.None);

            return Ok(result);
        }

        [Authorize(Roles="Admin")]
        [HttpPost("mandators/{integrationId}")]
        public async Task<IActionResult> SendMandators([FromBody] MasterDataMandatorsDTO dto, Guid integrationId)
        {
            // var result = await _externalService.SendMandatorAsync(dto, integrationId);

            var response = await _masterDataService.HandleMandatorsAsync(dto);

            return Ok(response);
        }

    }
}