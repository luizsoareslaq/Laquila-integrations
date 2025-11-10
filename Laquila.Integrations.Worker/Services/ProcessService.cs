using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Worker.Querys.Interfaces;
using Laquila.Integrations.Worker.Services.Interfaces;

namespace Laquila.Integrations.Worker.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IEverest30Query _everestQuery;
        private readonly ILogger<ProcessService> _logger;
        private Guid maersk_integration_id = Guid.Parse(Environment.GetEnvironmentVariable("MAERSK_INTEGRATION_ID") ?? string.Empty);

        public ProcessService(IEverest30Query everest30Query
                            , ILogger<ProcessService> logger)
        {
            _everestQuery = everest30Query;
            _logger = logger;
        }

        public async Task ProcessAsync(CancellationToken ct)
        {
            /**********************************************************
            ************************** ITENS **************************
            **********************************************************/
            var items = await _everestQuery.GetItems(ct, 100);

            if (items?.ItemAttributes?.Count > 0)
                await _everestQuery.SendItems(ct, items, maersk_integration_id);


            /**************************************************************
            ************************** CADASTROS **************************
            **************************************************************/
            var cadastros = await _everestQuery.GetMandators(ct, 100);

            if (cadastros?.Mandators?.Count > 0)
                await _everestQuery.SendMandators(ct, cadastros, maersk_integration_id);

            

        }
    }
}