using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<List<MasterDataMandatorsDTO>> GetUnsentMandatorAsync();
        Task<List<MasterDataItemsPackageDTO>> GetUnsentItemsAsync();
    }
}