using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
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
            var prenotas = await GetOrders(stoppingToken);

            if (prenotas.Total > 0)
            {
                foreach (var item in prenotas.Items)
                {
                    await SendOrders(item, stoppingToken, Guid.Parse("F4C3C856-7D85-406F-9C29-08DDE65179AA"));

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task<PagedResult<PrenotaDTO>> GetOrders(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IEverest30Query>();

        var filters = new Laquila.Integrations.Core.Domain.Filters.LAQFilters
        {
            Page = 1,
            PageSize = 10,
            LoIniGenTime = DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),
            LoEndGenTime = DateOnly.FromDateTime(DateTime.Now)
        };

        return await query.GetOrders(filters, ct);
    }

    private async Task SendOrders(PrenotaDTO dto, CancellationToken ct, Guid IntegrationId)
    {
        using var scope = _scopeFactory.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IEverest30Query>();

        var filters = new Laquila.Integrations.Core.Domain.Filters.LAQFilters
        {
            Page = 1,
            PageSize = 10,
            LoIniGenTime = DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),
            LoEndGenTime = DateOnly.FromDateTime(DateTime.Now)
        };

        await query.SendOrders(dto, ct, IntegrationId);
    }

}
