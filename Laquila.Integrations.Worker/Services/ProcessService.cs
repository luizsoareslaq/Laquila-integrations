using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.Filters;
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
            var items = await _everestQuery.MasterData.GetItemsAsync(ct, 100);

            if (items?.ItemAttributes?.Count > 0)
                await _everestQuery.MasterData.SendItemsAsync(items, maersk_integration_id, ct);

            /**************************************************************
            ************************** CADASTROS **************************
            **************************************************************/
            var cadastros = await _everestQuery.MasterData.GetMandatorsAsync(ct, 100);

            if (cadastros?.Mandators?.Count > 0)
                await _everestQuery.MasterData.SendMandatorsAsync(cadastros, maersk_integration_id, ct);

            /**************************************************************
            ************************** PRENOTAS ***************************
            **************************************************************/

            LAQFilters filters = new LAQFilters
            {
                Page = 1,
                PageSize = 10
            };

            var prenotas = await _everestQuery.Outbound.GetOrdersAsync(filters, ct);

            if (prenotas.Items.Count() > 0)
            {
                foreach (var prenota in prenotas.Items)
                {
                    var response = await _everestQuery.Outbound.SendOrdersAsync(prenota, maersk_integration_id, ct);

                    if (response != null && response.Errors == null)
                        _logger.LogInformation("Romaneio {0} enviado com sucesso.", prenota.LoOe);
                    else
                        _logger.LogError("Erro ao enviar romaneio {0}.", prenota.LoOe);
                }
            }

            /**************************************************************
            ************************ RECEBIMENTOS *************************
            **************************************************************/

            var invoices = await _everestQuery.Inbound.GetReceiveInvoicesAsync(filters, ct);

        }
    }
}