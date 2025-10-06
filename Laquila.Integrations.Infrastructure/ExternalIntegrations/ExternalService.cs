using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Infra.Interfaces;
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

        public async Task<ResponseDto> SendPrenotasAsync(PrenotaDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendExternalOrders";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            try
            {
                var client = new RestClient(urls.Url);
                var request = new RestRequest(urls.Url, Method.Post);

                request.AddHeader("Content-Type", "application/json");

                // Caso haja token configurado na URL de integração
                if (!string.IsNullOrEmpty(urls.AuthType) && urls.AuthType.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    // Exemplo: request.AddHeader("Authorization", $"Bearer {token}");
                }

                // Adiciona o corpo JSON
                request.AddJsonBody(dto);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                }

                // Chamar procedure de envio de prenota no EVEREST30
                // await _queueService.EnqueueAsync("UpdateERPOrder", dto.LoOe.ToString(), UserContext.CompanyCnpj);

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
                errors.Add("SendOrder", "lo_oe", dto.LoOe.ToString(),
                    $"Order {dto.LoOe} could not be sent because: {ex.Message}", true);

                throw;
            }
        }
    }
}