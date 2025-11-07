using System.Reflection;
using System.Text.RegularExpressions;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Worker.Context;
using Laquila.Integrations.Worker.DTO;
using Laquila.Integrations.Worker.Querys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Laquila.Integrations.Worker.Querys
{
    public class Everest30Query : IEverest30Query
    {
        private readonly AuthContext _context;
        protected readonly ErrorCollector errors = new ErrorCollector();

        public Everest30Query(AuthContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<PrenotaDTO>> GetOrders(LAQFilters filters, CancellationToken ct)
        {
            var authType = "Bearer";
            TokenAuthDTO authDto = new TokenAuthDTO();

            if (authType != null)
            {
                var token = await GetValidTokenAsync(ct);
                authDto.Token = token;
            }

            (RestClient client, RestRequest request) = NewRestSharpClient("https://localhost:5001/api/outbound/orders", null, filters, authType, authDto, "get");

            var response = await client.ExecuteAsync<PagedResult<PrenotaDTO>>(request);

            var retorno = response.Data;

            return retorno;
        }

        public async Task<ResponseDto> SendOrders(PrenotaDTO dto, CancellationToken ct, Guid apiIntegrationId)
        {
            var authType = "Bearer";
            TokenAuthDTO authDto = new TokenAuthDTO();

            if (authType != null)
            {
                var token = await GetValidTokenAsync(ct);
                authDto.Token = token;
            }

            (RestClient client, RestRequest request) = NewRestSharpClient($"https://localhost:5001/api/outbound/orders/external/{apiIntegrationId}", dto, null, authType, authDto, "post");

            var response = await client.ExecuteAsync<ResponseDto>(request);

            var retorno = response.Data;

            return retorno;
        }

        private static (RestClient, RestRequest) NewRestSharpClient(
            string url,
            object? body,
            object? queryParams,
            string? authType,
            TokenAuthDTO? authDto,
            string methodType)
        {
            var client = new RestClient(url);

            var method = methodType.ToLower() switch
            {
                "get" => Method.Get,
                "post" => Method.Post,
                "put" => Method.Put,
                "patch" => Method.Patch,
                "delete" => Method.Delete,
                _ => Method.Post
            };

            var request = new RestRequest(url, method);
            request.AddHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(authType) && authType.Equals("Bearer", StringComparison.OrdinalIgnoreCase) && authDto != null)
            {
                request.AddHeader("Authorization", $"Bearer {authDto.Token}");
            }

            if (queryParams != null)
            {
                var props = queryParams.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    var value = prop.GetValue(queryParams);
                    if (value == null) continue;

                    var attr = prop.GetCustomAttribute<FromQueryAttribute>();
                    var paramName = attr?.Name ?? ToSnakeCase(prop.Name);

                    if (value is DateOnly dateOnly)
                        value = dateOnly.ToString("yyyy-MM-dd");
                    else if (value is DateTime dateTime)
                        value = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");

                    request.AddQueryParameter(paramName, value.ToString());
                }
            }

            if (body != null)
                request.AddJsonBody(body);

            return (client, request);
        }

        private static string ToSnakeCase(string input) =>
            Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();

        public async Task<string> GetValidTokenAsync(CancellationToken ct)
        {
            if (_context.IsValid())
                return _context.GetToken().Token!;

            var company_cnpj = "03902443000670";
            LoginRequestDto login = new LoginRequestDto("luiz.soares", "laquilateste", "03902443000670");

            (RestClient client, RestRequest request) = NewRestSharpClient($"https://localhost:5001/api/auth/login", login, null, null, null, "post");

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