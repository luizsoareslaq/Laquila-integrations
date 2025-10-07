using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Worker.Querys.Interfaces;

namespace Laquila.Integrations.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var query = scope.ServiceProvider.GetRequiredService<IEverest30Query>();

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var filters = new Laquila.Integrations.Core.Domain.Filters.LAQFilters
            {
                Page = 1,
                PageSize = 10,
                LoIniGenTime = DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),
                LoEndGenTime = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = await query.GetPrenotas(filters, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
