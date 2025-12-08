using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Worker.Context;
using RestSharp;

namespace Laquila.Integrations.Worker.Infrastructure.Auth
{
    public class EverestAuthService : IEverestAuthService
    {
        private readonly AuthContext _context;
        protected readonly ErrorCollector errors = new ErrorCollector();
        private string urlBase;

        public EverestAuthService(AuthContext context
                            , IHostEnvironment env)
        {
            _context = context;
            urlBase = env.IsDevelopment() ? Environment.GetEnvironmentVariable("BASE_DEV_URL") ?? string.Empty : Environment.GetEnvironmentVariable("BASE_PROD_URL") ?? string.Empty;
        }

        public async Task<string> GetValidTokenAsync(CancellationToken ct)
        {
            if (_context.IsValid())
                return _context.GetToken().Token!;

            var company_cnpj = "03902443000670";
            LoginRequestDto login = new LoginRequestDto("luiz.soares", "laquilateste", "03902443000670");

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient($"{urlBase}/auth/login", login, null, null, null, "post");

            var response = await client.ExecuteAsync<TokenResponseDto>(request);

            var retorno = response.Data;

            if (response.IsSuccessStatusCode && retorno != null)
            {
                _context.SetToken(retorno.JWTToken, DateTime.Now.AddSeconds(retorno.ExpiresIn), company_cnpj);
            }
            else
            {
                errors.Add("AuthWorker", "", "",
                    $"Authentication failed. - {response.ErrorMessage}", true);

                return string.Empty;
            }

            return retorno.JWTToken;
        }
    }
}