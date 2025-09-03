using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Services;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Infrastructure.Repositories;

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

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApiIntegrationsRepository, ApiIntegrationsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }
    }
}