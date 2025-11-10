using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Worker.Context;
using Laquila.Integrations.Worker.Querys.Interfaces;
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

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient("https://localhost:5001/api/outbound/orders", null, filters, authType, authDto, "get");

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

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient($"https://localhost:5001/api/outbound/orders/external/{apiIntegrationId}", dto, null, authType, authDto, "post");

            var response = await client.ExecuteAsync<ResponseDto>(request);

            var retorno = response.Data;

            return retorno;
        }

        public async Task<MasterDataItemsPackageDTO> GetItems(CancellationToken ct, int pageSize = 100)
        {
            var authType = "Bearer";
            TokenAuthDTO authDto = new TokenAuthDTO();

            if (authType != null)
            {
                var token = await GetValidTokenAsync(ct);
                authDto.Token = token;
            }

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient($"https://localhost:5001/api/masterdata/items", null, pageSize, authType, authDto, "get");

            var response = await client.ExecuteAsync<MasterDataItemsPackageDTO>(request);

            var retorno = response.Data;

            return retorno;
        }

        public async Task<MasterDataItemsPackageDTO> SendItems(CancellationToken ct, MasterDataItemsPackageDTO dto, Guid integrationId)
        {
            var authType = "Bearer";
            TokenAuthDTO authDto = new TokenAuthDTO();

            if (authType != null)
            {
                var token = await GetValidTokenAsync(ct);
                authDto.Token = token;
            }

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient($"https://localhost:5001/api/masterdata/items/{integrationId}", dto, null, authType, authDto, "post");

            var response = await client.ExecuteAsync<MasterDataItemsPackageDTO>(request);

            var retorno = response.Data;

            return retorno;
        }


        public async Task<string> GetValidTokenAsync(CancellationToken ct)
        {
            if (_context.IsValid())
                return _context.GetToken().Token!;

            var company_cnpj = "03902443000670";
            LoginRequestDto login = new LoginRequestDto("luiz.soares", "laquilateste", "03902443000670");

            (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient($"https://localhost:5001/api/auth/login", login, null, null, null, "post");

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