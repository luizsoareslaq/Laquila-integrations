using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Services;
using Laquila.Integrations.Core.Domain.Mappings;
using Laquila.Integrations.Core.Domain.Services;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Infra.Repositories;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Infrastructure.Repositories;
using Laquila.Integrations.Core.Domain.Services.Interfaces;

namespace Laquila.Integrations.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApiIntegrationsService, ApiIntegrationsService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<INotaService, NotaService>();
            services.AddScoped<IRomaneioService, RomaneioService>();

            //Repositories
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApiIntegrationsRepository, ApiIntegrationsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<INotaRepository, NotaRepository>();
            services.AddScoped<IRomaneioRepository, RomaneioRepository>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}