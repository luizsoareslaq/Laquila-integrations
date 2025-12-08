using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Worker.Infrastructure.Auth;
using RestSharp;

namespace Laquila.Integrations.Worker.Infrastructure.Http
{
    public class EverestHttpClient : IEverestHttpClient
    {
        private readonly IEverestAuthService _authService;
        private readonly string _baseUrl;

        public EverestHttpClient(IEverestAuthService authService, IHostEnvironment env)
        {
            _authService = authService;

            _baseUrl = env.IsDevelopment()
                ? Environment.GetEnvironmentVariable("BASE_DEV_URL") ?? string.Empty
                : Environment.GetEnvironmentVariable("BASE_PROD_URL") ?? string.Empty;
        }

        private async Task<RestClient> BuildClientAsync(RestRequest request, string httpMethod, CancellationToken ct)
        {
            var token = await _authService.GetValidTokenAsync(ct);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.Method = httpMethod switch
            {
                "GET" => Method.Get,
                "POST" => Method.Post,
                "PATCH" => Method.Patch,
                "PUT" => Method.Put,
                _ => Method.Get
            };

            return new RestClient(_baseUrl);
        }

        private static void AddQueryParams(RestRequest req, object? query)
        {
            if (query == null) return;

            foreach (var prop in query.GetType().GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(query);

                if (value != null)
                    req.AddQueryParameter(name, value.ToString());
            }
        }

        private static void AddBody(RestRequest req, object? body)
        {
            if (body != null)
                req.AddJsonBody(body);
        }

        private async Task<T?> ExecuteAsync<T>(string endpoint, string method, object? query, object? body, CancellationToken ct)
        {
            var req = new RestRequest(endpoint);

            AddQueryParams(req, query);
            AddBody(req, body);

            var client = await BuildClientAsync(req, method, ct);

            var response = await client.ExecuteAsync<T>(req, ct);

            if (!response.IsSuccessStatusCode)
            {
                // Enviar email ou apontar log para nova API.
                // throw new Exception($"{response.StatusCode} - {response.ErrorMessage}");
            }

            return response.Data;
        }

        public Task<T?> GetAsync<T>(string endpoint, object? query = null, CancellationToken ct = default)
            => ExecuteAsync<T>(endpoint, "GET", query, null, ct);

        public Task<T?> PostAsync<T>(string endpoint, object? body, CancellationToken ct = default)
            => ExecuteAsync<T>(endpoint, "POST", null, body, ct);

        public Task<T?> PatchAsync<T>(string endpoint, object? body, CancellationToken ct = default)
            => ExecuteAsync<T>(endpoint, "PATCH", null, body, ct);

        public Task<T?> PutAsync<T>(string endpoint, object? body, CancellationToken ct = default)
            => ExecuteAsync<T>(endpoint, "PUT", null, body, ct);
    }
}