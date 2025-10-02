using System.Diagnostics;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Enums.Everest30;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Application.Services
{
    public class PrenotaService : IPrenotaService
    {
        private readonly IPrenotaRepository _prenotaRepository;
        private readonly IQueueService _queueService;
        private readonly IEverest30Service _everest30Service;
        public PrenotaService(IPrenotaRepository prenotaRepository
                            , IQueueService queueService
                            , IEverest30Service everest30Service)
        {
            _prenotaRepository = prenotaRepository;
            _queueService = queueService;
            _everest30Service = everest30Service;
        }

        public async Task<PagedResult<PrenotaDTO>> GetPrenotasAsync(LAQFilters filters, CancellationToken ct)
        {
            (var prenotasList, int TotalCount) = await _prenotaRepository.GetPrenotasAsync(filters, ct);

            var response = new PagedResult<PrenotaDTO>() { };

            response.Total = TotalCount;
            response.Page = filters.Page;
            response.PageSize = filters.PageSize;

            var prenotas = new List<PrenotaDTO>();

            if (prenotasList.Count() > 0)
            {
                prenotas = prenotasList.GroupBy(x => x.LoOe).Select(grupo =>
                {
                    var prenotaBase = grupo.First();
                    return new PrenotaDTO
                    {
                        LoMaCnpjOwner = prenotaBase.LoMaCnpjOwner,
                        LoMaCnpj = prenotaBase.LoMaCnpj,
                        LoMaCnpjCarrier = prenotaBase.LoMaCnpjCarrier,
                        LoMaCnpjRedespacho = prenotaBase.LoMaCnpjRedespacho,
                        LoPriority = prenotaBase.LoPriority,
                        LoType = prenotaBase.LoType,
                        LoOe = prenotaBase.LoOe,
                        OeErpOrder = prenotaBase.OeErpOrder,
                        Items = grupo.Select(item => new PrenotaItemsDTO
                        {
                            OelId = item.OelId,
                            OelAtId = item.OelAtId,
                            OelQtyReq = item.OelQtyReq
                        }).ToList()
                    };
                }).ToList();

                response.Items = prenotas;
            }

            return response;
        }

        public async Task<ResponseDto> UpdatePrenotasStatusAsync(long lo_oe, PrenotaDatesDTO dto)
        {
            var loadOut = await _everest30Service.GetLoadOutByLoOe(lo_oe, UserContext.CompanyCnpj ?? string.Empty);

            bool continueProcess = loadOut.LoStatus switch
            {
                var status when status != (int)LoStatusEnum.Separacao && status != (int)LoStatusEnum.Conferencia => false,
                _ => true
            };

            var errors = new List<ResponseErrorsDto>();

            if (!continueProcess)
            {
                errors.Add(new ResponseErrorsDto
                {
                    StatusCode = 400,
                    Entity = "Order",
                    Key = lo_oe.ToString(),
                    Value = "",
                    Message = $"The order {lo_oe} is not in a valid status for updating dates."
                });
            }

            if (errors.Any())
                throw new ResponseErrorException(errors);

            try
            {
                var queue = await _queueService.EnqueueAsync("OrdersDate", "lo_oe", lo_oe.ToString(), dto, ApiStatusEnum.Pending, 1);

                return new ResponseDto()
                {
                    Data = new ResponseDataDto()
                    { Message = $"The status update for order {lo_oe} was queued successfully with id {queue}.", StatusCode = "200" }
                };
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Task<ResponseDto> UpdateRenouncedItemsAsync(long lo_oe, PrenotaRenouncedDTO dto)
        {
            return null;
        }
    }
}