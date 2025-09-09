using Laquila.Integrations.Core.Domain.DTO.Romaneio.Response;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Core.Domain.Services
{
    public class RomaneioService : IRomaneioService
    {
        private readonly IRomaneioRepository _romaneioRepository;
        public RomaneioService(IRomaneioRepository romaneioRepository)
        {
            _romaneioRepository = romaneioRepository;
        }
        public async Task<PagedResult<RomaneioResponseDTO>> GetRomaneiosAsync(LAQFilters filters, CancellationToken ct)
        {
            (var romaneiosList, int TotalCount) = await _romaneioRepository.GetRomaneiosAsync(filters, ct);

            var response = new PagedResult<RomaneioResponseDTO>() { };

            response.Total = TotalCount;
            response.Page = filters.Page;
            response.PageSize = filters.PageSize;

            List<RomaneioResponseDTO> romaneiosDto = new List<RomaneioResponseDTO>();

            var romaneiosAgrupados = romaneiosList
                            .GroupBy(x => x.IdRomaneio)
                            .Select(grupo =>
                            {
                                var romaneioBase = grupo.First();

                                return new RomaneioResponseDTO
                                {
                                    CnpjEmpresa = romaneioBase.CnpjEmpresa,
                                    CdEmpresa = romaneioBase.CdEmpresa,
                                    IdRomaneio = romaneioBase.IdRomaneio,
                                    DhEmissaoRomaneio = romaneioBase.DhEmissao,
                                    CdCliente = romaneioBase.CdCliente,
                                    CpfCnpjCliente = romaneioBase.CpfCnpjCliente,
                                    RazaoCliente = romaneioBase.RazaoCliente,
                                    FantasiaCliente = romaneioBase.FantasiaCliente,
                                    CdTransportador = romaneioBase.CdTransportador,
                                    CnpjTransportadora = romaneioBase.CnpjTransportadora,
                                    RazaoTransportadora = romaneioBase.RazaoTransportadora,
                                    AtSituacaoRomaneio = romaneioBase.AtSituacaoRomaneio,
                                    SituacaoRomaneio = romaneioBase.SituacaoRomaneio,
                                    Itens = grupo.OrderBy(x => x.IdRomaneioDocumentoItem)
                                                 .Select(item => new ItensRomaneioDTO()
                                                 {
                                                     CdItem = item.CdItem,
                                                     DsItem = item.DsItem,
                                                     IdEmbalagem = item.IdEmbalagem,
                                                     DsEmbalagem = item.DsEmbalagem,
                                                     QtItem = item.QtItem,
                                                     QtBaixa = item.QtBaixa,
                                                     QtCortada = item.QtCortada,
                                                     QtFaturada = item.QtFaturada,
                                                     Mensagem = item.Mensagem
                                                 })
                                };
                            }).ToList();

            response.Items = romaneiosAgrupados;

            return response;
        }
    }
}