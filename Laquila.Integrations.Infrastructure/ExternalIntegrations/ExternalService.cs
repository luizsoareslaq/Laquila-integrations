using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RestSharp;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.ExternalServices
{
    public class ExternalService : IExternalService
    {
        protected readonly ErrorCollector errors = new ErrorCollector();
        private readonly IApiIntegrationsRepository _integrationsRepository;
        private readonly IQueueService _queueService;
        private string? IntegrationType;
        public ExternalService(IApiIntegrationsRepository integrationsRepository
                             , IQueueService queueService)
        {
            _integrationsRepository = integrationsRepository;
            _queueService = queueService;
        }

        public async Task<ResponseDto> SendOrdersAsync(PrenotaDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendExternalOrders";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            try
            {
                (RestClient client,RestRequest request) = NewRestSharpClient(urls.Url, dto, urls.AuthType);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                }

                // Chamar procedure de envio de prenota no EVEREST30
                var queue = await _queueService.EnqueueAsync("UpdateERPOrder", "lo_oe", dto.LoOe.ToString(), response, ApiStatusEnum.Pending, 1);

                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = ((int)response.StatusCode).ToString(),
                        Message = $"Prenota sent successfully to external system. ({response.StatusCode})"
                    }
                };
            }
            catch (Exception ex)
            {
                var queue = await _queueService.EnqueueAsync("UpdateERPOrder", "lo_oe", dto.LoOe.ToString(), ex.Message + " - " + ex.InnerException, ApiStatusEnum.Error, 1);

                errors.Add("SendOrder", "lo_oe", dto.LoOe.ToString(),
                    $"Order {dto.LoOe} could not be sent because: {ex.Message + " - " + ex.InnerException}", true);

                throw;
            };
        }

        public async Task<ResponseDto> SendInvoicesAsync(PrenotaDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendExternalOrders";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            try
            {
                (RestClient client,RestRequest request) = NewRestSharpClient(urls.Url, dto, urls.AuthType);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                }

                // Chamar procedure de envio de prenota no EVEREST30
                var queue = await _queueService.EnqueueAsync("UpdateERPOrder", "lo_oe", dto.LoOe.ToString(), response, ApiStatusEnum.Pending, 1);

                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = ((int)response.StatusCode).ToString(),
                        Message = $"Prenota sent successfully to external system. ({response.StatusCode})"
                    }
                };
            }
            catch (Exception ex)
            {
                var queue = await _queueService.EnqueueAsync("UpdateERPOrder", "lo_oe", dto.LoOe.ToString(), ex.Message + " - " + ex.InnerException, ApiStatusEnum.Error, 1);

                errors.Add("SendOrder", "lo_oe", dto.LoOe.ToString(),
                    $"Order {dto.LoOe} could not be sent because: {ex.Message + " - " + ex.InnerException}", true);

                throw;
            }
        }

        private static (RestClient,RestRequest) NewRestSharpClient(string url, object dto, string authType)
        {
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(authType) && authType.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                // Exemplo: request.AddHeader("Authorization", $"Bearer {token}");
            }

            request.AddJsonBody(dto);

            return (client, request);
        }
    }
}