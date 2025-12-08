using Laquila.Integrations.API.Logging;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Interfaces.Everest30;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Application.Services.Everest30;
using Laquila.Integrations.Application.Services.LaqHub;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Domain.Interfaces.Repositories.Everest30;
using Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub;
using Laquila.Integrations.Infrastructure.ExternalServices;
using Laquila.Integrations.Infrastructure.Repositories;
using Laquila.Integrations.Infrastructure.Repositories.Everest30;
using Laquila.Integrations.Infrastructure.Repositories.LaqHub;

namespace Laquila.Integrations.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {

            //Logging
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddHostedService<LogWorker>();
            
            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApiIntegrationsService, ApiIntegrationsService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalService, ExternalService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IInboundService, InboundService>();

            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IEverest30Service, Everest30Service>();

            //Repositories
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApiIntegrationsRepository, ApiIntegrationsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IQueueRepository, QueueRepository>();

            services.AddScoped<IViewsRepository, ViewsRepository>();
            services.AddScoped<IEverest30Repository, Everest30Repository>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();

            services.AddMemoryCache();

            return services;
        }
    }
}