using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Worker.Querys;
using Laquila.Integrations.Worker.Querys.Interfaces;

namespace Laquila.Integrations.Worker.Configurations
{
    public static class QueryDependencyInjection
    {
        public static IServiceCollection AddQuerys(this IServiceCollection services)
        {
            services.AddSingleton<IEverest30Query, Everest30Query>();

            return services;
        }
    }
}