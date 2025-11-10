using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Worker.Querys.Interfaces
{
    public interface IEverest30Query
    {
        Task<PagedResult<PrenotaDTO>> GetOrders(LAQFilters filters, CancellationToken ct);
        Task<ResponseDto> SendOrders(PrenotaDTO dto, CancellationToken ct, Guid apiIntegrationId);

        Task<MasterDataItemsPackageDTO> GetItems(CancellationToken ct, int pageSize = 100);
        Task<MasterDataItemsPackageDTO> SendItems(CancellationToken ct, MasterDataItemsPackageDTO dto, Guid integrationId);

        Task<MasterDataMandatorsDTO> GetMandators(CancellationToken ct, int pageSize = 100);
        Task<MasterDataMandatorsDTO> SendMandators(CancellationToken ct, MasterDataMandatorsDTO dto, Guid integrationId);
    }
}