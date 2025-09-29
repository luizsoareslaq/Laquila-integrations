using Laquila.Integrations.Core.Domain.DTO.Prenota.Response;
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

        public async Task<PagedResult<List<PrenotaResumoDTO>>> GetPrenotasAsync(LAQFilters filters, CancellationToken ct)
        {
            (var prenotasList, int TotalCount) = await _prenotaRepository.GetPrenotasAsync(filters, ct);

            var response = new PagedResult<List<PrenotaResumoDTO>>() { };

            response.Total = TotalCount;
            response.Page = filters.Page;
            response.PageSize = filters.PageSize;

            var prenotasAgrupados = prenotasList.GroupBy(x => x.LoOe).Select(grupo =>
            {
                var prenotaBase = grupo.First(); return new PrenotaResumoDTO
                {
                    LoOe = prenotaBase.LoOe,
                    LoMaCnpjOwner = prenotaBase.LoMaCnpjOwner
                };
            }).ToList();

            response.Items = (IEnumerable<List<PrenotaResumoDTO>>)prenotasAgrupados;

            return response;
        }
    }
}