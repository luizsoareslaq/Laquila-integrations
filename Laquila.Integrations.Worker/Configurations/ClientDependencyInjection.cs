using Laquila.Integrations.Worker.Clients.Inbound;
using Laquila.Integrations.Worker.Clients.MasterData;
using Laquila.Integrations.Worker.Clients.Outbound;
using Laquila.Integrations.Worker.Infrastructure.Http;

namespace Laquila.Integrations.Worker.Configurations
{
    public static class ClientDependencyInjection 
    {
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddSingleton<IEverestHttpClient, EverestHttpClient>();

            services.AddSingleton<IOutboundClient, OutboundClient>();
            services.AddSingleton<IMasterDataClient, MasterDataClient>();
            services.AddSingleton<IInboundClient, InboundClient>();

            return services;
        }
    }
}