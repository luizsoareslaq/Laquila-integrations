using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<MasterDataMandatorsDTO> GetUnsentMandatorAsync(MasterDataFilters filters, CancellationToken ct);
        Task<MasterDataItemsPackageDTO> GetUnsentItemsAsync(MasterDataFilters filters, CancellationToken ct);

        Task<ResponseDto> HandleItemsAsync(MasterDataItemsPackageDTO dto);
        Task<ResponseDto> HandleMandatorsAsync(MasterDataMandatorsDTO dto);
    }
}