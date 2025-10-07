using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Services;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Infra.Repositories;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Infrastructure.Contexts;
using Laquila.Integrations.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Laquila.Integrations.Bootstrap.DependencyInjection
{
    public static class CoreDependencyInjection
    {
        public static IServiceCollection AddBootstrapServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("Everest30Connection") ?? "";

            services.AddDbContext<LaquilaHubContext>(options =>
                options.UseSqlServer(connString));

            services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connString));

            services.AddScoped<IPrenotaService, PrenotaService>();
            services.AddScoped<IQueueService, QueueService>();

            services.AddScoped<IPrenotaRepository, PrenotaRepository>();
            services.AddScoped<IQueueRepository, QueueRepository>();

            return services;
        }
    }
}