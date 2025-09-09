using System.Security.Cryptography.X509Certificates;
using Laquila.Integrations.Core.Domain.DTO.Notas.Response;
using Laquila.Integrations.Core.Domain.DTO.Notas.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Core.Domain.Services
{
    public class NotaService : INotaService
    {
        private readonly INotaRepository _notaRepository;
        public NotaService(INotaRepository notaRepository)
        {
            _notaRepository = notaRepository;
        }

        public async Task<PagedResult<NotasResponseDTO>> GetNotasAsync(LAQFilters filters, CancellationToken ct)
        {
            (var notasList, int TotalCount) = await _notaRepository.GetNotasAsync(filters, ct);

            var response = new PagedResult<NotasResponseDTO>() { };

            response.Total = TotalCount;
            response.Page = filters.Page;
            response.PageSize = filters.PageSize;

            List<NotasResponseDTO> notasDto = new List<NotasResponseDTO>();

            var notasAgrupadas = notasList
                    .GroupBy(x => x.IdNotaEmitida)
                    .Select(grupo =>
                    {
                        var notaBase = grupo.First();

                        return new NotasResponseDTO
                        {
                            CpfCnpjEmpresa = notaBase.CpfCnpjEmpresa,
                            CdEmpresa = notaBase.CdEmpresa,
                            IdNotaEmitida = notaBase.IdNotaEmitida,
                            NrNota = notaBase.NrNota,
                            CdChaveAcessoNota = notaBase.CdChaveAcessoNota,
                            DhEmissaoNota = notaBase.DhEmissao,
                            CdCliente = notaBase.CdCliente,
                            CpfCnpjCliente = notaBase.CpfCnpjCliente,
                            RazaoCliente = notaBase.RazaoCliente,
                            FantasiaCliente = notaBase.FantasiaCliente,
                            VlMercadoriaNota = notaBase.VlMercadoriaNota,
                            VlTotalNota = notaBase.VlTotalNota,
                            CdTransportador = notaBase.CdTransportador,
                            CnpjTransportadora = notaBase.CnpjTransportadora,
                            RazaoTransportadora = notaBase.RazaoTransportadora,
                            CdRedespacho = notaBase.CdRedespacho,
                            CnpjRedespacho = notaBase.CnpjRedespacho,
                            RazaoRedespacho = notaBase.RazaoRedespacho,
                            AtSituacaoNota = notaBase.AtSituacaoNota,
                            SituacaoNota = notaBase.SituacaoNota,
                            Itens = grupo.OrderBy(x=> x.NrOrdem).Select(item => new ItensNotaDTO
                            {
                                NrOrdem = item.NrOrdem,
                                CdItem = item.CdItem,
                                DsItem = item.DsItem,
                                IdEmbalagem = item.IdEmbalagem,
                                DsEmbalagem = item.DsEmbalagem,
                                QtItem = item.QtItem,
                                VlTotalItem = item.VlTotalItem
                            })
                        };
                    })
                    .ToList();

            response.Items = notasAgrupadas;

            return response;
        }
    }
}