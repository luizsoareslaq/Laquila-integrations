using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.API.Middlewares;
using Laquila.Integrations.Application.Interfaces.LaqHub;

namespace Laquila.Integrations.API.Logging
{
    public class LogWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LogWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var reader = LogChannel.Channel.Reader;

            while (!stoppingToken.IsCancellationRequested)
            {
                var log = await reader.ReadAsync(stoppingToken);

                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var service = scope.ServiceProvider.GetRequiredService<ILogService>();
                    await service.HandleLogAsync(log);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao processar log: " + ex.Message);
                }
            }
        }
    }
}