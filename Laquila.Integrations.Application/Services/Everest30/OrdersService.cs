using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces.Everest30;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Application.Validators;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Enums.Everest30;

namespace Laquila.Integrations.Application.Services.Everest30
{
    public class OrdersService : IOrdersService
    {
        private readonly IViewsRepository _viewsRepository;
        private readonly IQueueService _queueService;
        private readonly IEverest30Service _everest30Service;
        protected readonly ErrorCollector errors = new ErrorCollector();

        public OrdersService(IViewsRepository viewsRepository
                            , IQueueService queueService
                            , IEverest30Service everest30Service)
        {
            _viewsRepository = viewsRepository;
            _queueService = queueService;
            _everest30Service = everest30Service;
        }

        public async Task<PagedResult<PrenotaDTO>> GetUnsentOrdersAsync(LAQFilters filters, CancellationToken ct)
        {
            (var prenotasList, int TotalCount) = await _viewsRepository.GetOrdersAsync(new List<(string, long)>(), filters.PageSize);

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

        public async Task<ResponseDto> UpdateOrderStatusAsync(long lo_oe, PrenotaDatesDTO dto)
        {
            var loadOut = await _everest30Service.GetLoadOutByLoOe(lo_oe);

            if (!OrderValidator.CanUpdateStatus(loadOut.LoStatus))
            {
                errors.Add("Order", "lo_oe", lo_oe.ToString(),
                    string.Format(MessageProvider.Get("OrderInvalidStatus", UserContext.Language), lo_oe), true);
            }

            var dateValidation = OrderValidator.CanUpdateDates(loadOut, dto);

            if (!dateValidation.canUpdate)
            {
                errors.Add("Order", "lo_oe", lo_oe.ToString(), dateValidation.message, true);
            }

            dto.LoStatusAtual = loadOut.LoStatus;

            var queue = await _queueService.EnqueueAsync("LoadOutDates", "lo_oe", lo_oe.ToString(), dto, ApiStatusEnum.Pending, 1, null);

            return new ResponseDto()
            {
                Data = new ResponseDataDto()
                { Message = string.Format(MessageProvider.Get("OrderQueued", UserContext.Language), lo_oe, queue), StatusCode = "200" }
            };
        }

        public async Task<ResponseDto> UpdateRenouncedItemsAsync(long lo_oe, PrenotaRenouncedDTO dto)
        {
            var ordersLine = await _everest30Service.GetOeItemsByLoOe(lo_oe);

            var loStatus = ordersLine.FirstOrDefault()?.Orders.LoadOut.LoStatus ?? 0;

            if (!OrderValidator.CanUpdateConfStatus(loStatus))
            {
                errors.Add("Order", "lo_oe", lo_oe.ToString(),
                    string.Format(MessageProvider.Get("OrderInvalidStatusRenounced", UserContext.Language), lo_oe), true);
            }

            var itemsNotFound = dto.Items.Where(i => !ordersLine.Any(ol => ol.OelId == i.OelId)).ToList();

            int counter = 1;
            bool throwError = false;

            foreach (var item in itemsNotFound)
            {
                if (counter == itemsNotFound.Count)
                    throwError = true;

                errors.Add("OrderLine", "oel_id", item.OelId.ToString(), string.Format(MessageProvider.Get("ItemOelIdNotFound", UserContext.Language), item.OelId, lo_oe), throwError);

                counter++;
            }

            var orderLineItemsNotFoundInDto = ordersLine.Where(ol => !dto.Items.Any(i => i.OelId == ol.OelId)).ToList();

            counter = 1;
            throwError = false;
            foreach (var item in orderLineItemsNotFoundInDto)
            {
                if (counter == orderLineItemsNotFoundInDto.Count)
                    throwError = true;

                errors.Add("OrderLine", "oel_id", item.OelId.ToString(), string.Format(MessageProvider.Get("ItemOelIdNotFoundRequestBody", UserContext.Language), item.OelId, lo_oe), throwError);
            }

            //Adicionar fila para atualizar as renuncias
            var queue = await _queueService.EnqueueAsync("OrdersLine", "lo_oe", lo_oe.ToString(), dto, ApiStatusEnum.Pending, 1, null);

            return new ResponseDto()
            {
                Data = new ResponseDataDto()
                { Message = string.Format(MessageProvider.Get("ItemsUpdateSuccess", UserContext.Language), lo_oe), StatusCode = "204" }
            };
        }
    }
}