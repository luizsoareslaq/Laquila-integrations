using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces.Everest30;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Application.Services.Everest30
{
    public class InboundService : IInboundService
    {

        private readonly IViewsRepository _viewsRepository;
        private readonly IQueueService _queueService;
        private readonly IEverest30Service _everest30Service;
        protected readonly ErrorCollector errors = new ErrorCollector();
        private readonly string lang = UserContext.Language ?? "en";
        public InboundService(IViewsRepository viewsRepository
                            , IQueueService queueService
                            , IEverest30Service everest30Service)
        {
            _viewsRepository = viewsRepository;
            _queueService = queueService;
            _everest30Service = everest30Service;
        }
        public async Task<PagedResult<ReceiveInvoiceDTO>> GetUnsentReceiveInvoicesAsync(LAQFilters filters, CancellationToken ct)
        {
             (var notasList, int TotalCount) = await _viewsRepository.GetReceiveInvoicesAsync(filters.PageSize);

            var response = new PagedResult<ReceiveInvoiceDTO>() { };

            response.Total = TotalCount;
            response.Page = filters.Page;
            response.PageSize = filters.PageSize;

            var notas = new List<ReceiveInvoiceDTO>();

            if (notasList.Count() > 0)
            {
                notas = notasList.Where(x=> int.TryParse(x.FnInvNumber, out int invNumber)).GroupBy(x => x.LiId).Select(grupo =>
                {
                    var notaBase = grupo.First();
                    return new ReceiveInvoiceDTO
                    {
                        LiId = notaBase.LiId,
                        LiMaCnpjOwner = notaBase.LiMaCnpjOwner,
                        LiPriority = notaBase.LiPriority,
                        FnMaCnpj = notaBase.FnMaCnpj,
                        FnInvNumber = int.TryParse(notaBase.FnInvNumber, out int invNumber) ? invNumber : 0,
                        FnType = notaBase.FnType,
                        FnSerialNr = notaBase.FnSerialNr,
                        Items = grupo.Select(item => new ReceiveInvoiceItemsDTO
                        {
                            FnlId = item.FnlId,
                            FnlAtId = item.FnlAtId,
                            FnlQty = item.FnlQty
                        }).ToList()
                    };
                }).ToList();

                response.Items = notas;
            }

            return response;
        }
    }
}