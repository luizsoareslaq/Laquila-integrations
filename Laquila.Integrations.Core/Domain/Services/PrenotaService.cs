using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Response;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Core.Domain.Services
{
    public class PrenotaService : IPrenotaService
    {
        private readonly IPrenotaRepository _prenotaRepository;
        public PrenotaService(IPrenotaRepository prenotaRepository)
        {
            _prenotaRepository = prenotaRepository;
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
    }
}