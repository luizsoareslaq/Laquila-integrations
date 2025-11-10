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

        public ProcessService(IEverest30Query everest30Query
                            , ILogger<ProcessService> logger)
        {
            _everestQuery = everest30Query;
            _logger = logger;
        }

        public async Task ProcessAsync(CancellationToken ct)
        {
            var items = await _everestQuery.GetItems(ct, 100);

            if (items?.ItemAttributes?.Count > 0)
            {
                var sendItems = await _everestQuery.SendItems(ct, items, Guid.Parse("F4C3C856-7D85-406F-9C29-08DDE65179AA"));
                _logger.LogInformation("Itens obtidos: {count}", items.ItemAttributes.Count);
            }
            else
            {
                _logger.LogInformation("Nenhum item encontrado no momento.");
            }
        }
    }
}