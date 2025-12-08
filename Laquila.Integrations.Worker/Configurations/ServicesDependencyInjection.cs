using Laquila.Integrations.Worker.Infrastructure.Auth;
using Laquila.Integrations.Worker.Services;
using Laquila.Integrations.Worker.Services.Interfaces;

namespace Laquila.Integrations.Worker.Configurations
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IEverestAuthService, EverestAuthService>();
            services.AddSingleton<IProcessService, ProcessService>();

            return services;
        }
    }
}