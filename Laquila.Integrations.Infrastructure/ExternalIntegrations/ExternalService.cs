using System.Text.Json;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub;
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
        private readonly string lang = UserContext.Language ?? "en";

        public ExternalService(IApiIntegrationsRepository integrationsRepository
                             , IQueueService queueService)
        {
            _integrationsRepository = integrationsRepository;
            _queueService = queueService;
        }

        public async Task<ResponseDto> SendMandatorAsync(MasterDataMandatorsDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendMandators";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            RestResponse response = new RestResponse();

            try
            {
                (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient(urls.Url, dto, null, urls.AuthType, null, urls.Type);

                response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                }


                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = ((int)response.StatusCode).ToString(),
                        Message = string.Format(MessageProvider.Get("MandatorsSent", lang))
                    }
                };
            }
            catch (Exception ex)
            {
                //Adicionar envio de email
                errors.Add("SendMandators", "error", "ma_code",
                    string.Format(MessageProvider.Get("MandatorsSentError", lang), ex.Message + " - " + ex.InnerException), true);

                throw;
            }
        }

        public async Task<ResponseDto> SendItemsAsync(MasterDataItemsPackageDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendItems";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            RestResponse response = new RestResponse();

            try
            {
                (RestClient client, RestRequest request) = RestSharpHelper.NewRestSharpClient(urls.Url, dto, null, urls.AuthType, null, urls.Type);

                response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                }


                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = ((int)response.StatusCode).ToString(),
                        Message = string.Format(MessageProvider.Get("ItemsSent", lang))
                    }
                };
            }
            catch (Exception ex)
            {
                //Adicionar envio de email


                errors.Add("SendItems", "error", "at_id",
                    string.Format(MessageProvider.Get("ItemsSentError", lang), ex.Message + " - " + ex.InnerException), true);

                throw;
            }
        }

        //OUTBOUND
        public async Task<ResponseDto> SendOrdersAsync(PrenotaDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendExternalOrders";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            RestResponse response = new RestResponse();

            try
            {
                // (RestClient client,RestRequest request) = NewRestSharpClient(urls.Url, dto, urls.AuthType);

                // var response = await client.ExecuteAsync(request);

                // if (!response.IsSuccessful)
                // {
                // throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                // }


                // Chamar procedure de envio de prenota no EVEREST30
                // var queue = await _queueService.EnqueueAsync("SendERPOrder", "lo_oe", dto.LoOe.ToString(), response, ApiStatusEnum.Pending, 1);
                var queue = await _queueService.EnqueueAsync("SendERPOrder", "lo_oe", dto.LoOe.ToString(), dto, ApiStatusEnum.Pending, 1, null);

                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = (200).ToString(),
                        Message = string.Format(MessageProvider.Get("OrderSent", lang), dto.LoOe)
                    }
                };
            }
            catch (Exception ex)
            {
                var queue = await _queueService.EnqueueAsync("SendERPOrder", "lo_oe", dto.LoOe.ToString(), response, ApiStatusEnum.Error, 1, ex.Message + " - " + ex.InnerException);

                errors.Add("SendOrder", "lo_oe", dto.LoOe.ToString(),
                    string.Format(MessageProvider.Get("OrderSentError", lang), dto.LoOe, ex.Message + " - " + ex.InnerException), true);

                throw;
            };
        }

        //OUTBOUND
        public async Task<ResponseDto> SendInvoicesAsync(InvoiceDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendExternalInvoices";

            // var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            RestResponse response = new RestResponse();

            // Fazendo desta forma pois o dto original está com o OeId preenchido, e não queremos enviar este campo para o sistema externo
            var dtoToSend = JsonSerializer.Deserialize<InvoiceDTO>(
                JsonSerializer.Serialize(dto)
            );
            dtoToSend.OeId = null;

            try
            {
                // (RestClient client,RestRequest request) = NewRestSharpClient(urls.Url, dtoToSend, urls.AuthType);

                // var response = await client.ExecuteAsync(request);

                // if (!response.IsSuccessful)
                // {
                // throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                // }


                // Chamar procedure de envio de prenota no EVEREST30
                var queue = await _queueService.EnqueueAsync("SendInvoiceOrder", "oe_id", dto.OeId.ToString(), dto, ApiStatusEnum.Pending, 1, null);

                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = ((int)response.StatusCode).ToString(),
                        Message = string.Format(MessageProvider.Get("InvoiceSent", lang), dto.OeInvNumber, dto.LoOe)
                    }
                };
            }
            catch (Exception ex)
            {
                var queue = await _queueService.EnqueueAsync("SendInvoiceOrder", "oe_id", dto.OeId.ToString(), response, ApiStatusEnum.Error, 1, ex.Message + " - " + ex.InnerException);

                errors.Add("SendOrder", "lo_oe", dto.LoOe.ToString(),
                    string.Format(MessageProvider.Get("InvoiceSentError", lang), dto.OeInvNumber, dto.LoOe, ex.Message + " - " + ex.InnerException), true);

                throw;
            }
        }

        //INBOUND
        public async Task<ResponseDto> SendReceiveInvoicesAsync(ReceiveInvoiceDTO dto, Guid apiIntegrationId)
        {
            IntegrationType = "SendReceiveInvoices";

            var urls = await _integrationsRepository.GetApiUrlIntegrationsByIntegrationIdAndEndpointKeyAsync(apiIntegrationId, IntegrationType);

            RestResponse response = new RestResponse();

            try
            {
                // (RestClient client,RestRequest request) = NewRestSharpClient(urls.Url, dto, urls.AuthType);

                // var response = await client.ExecuteAsync(request);

                // if (!response.IsSuccessful)
                // {
                // throw new Exception($"Integration failed ({response.StatusCode}): {response.Content}");
                // }


                // Chamar procedure de envio de prenota no EVEREST30
                var queue = await _queueService.EnqueueAsync("SendReceiveInvoices", "li_id", dto.LiId.ToString(), dto, ApiStatusEnum.Pending, 1, null);

                return new ResponseDto
                {
                    Data = new ResponseDataDto
                    {
                        StatusCode = "200",
                        Message = string.Format(MessageProvider.Get("OrderSent", lang), dto.LiId)
                    }
                };
            }
            catch (Exception ex)
            {
                var queue = await _queueService.EnqueueAsync("SendReceiveInvoices", "lo_oe", dto.LiId.ToString(), response, ApiStatusEnum.Error, 1, ex.Message + " - " + ex.InnerException);

                errors.Add("SendReceiveInvoices", "lo_oe", dto.LiId.ToString(),
                    string.Format(MessageProvider.Get("OrderSentError", lang), dto.LiId, ex.Message + " - " + ex.InnerException), true);

                throw;
            };
        }
    }
}