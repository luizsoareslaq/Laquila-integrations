using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;

namespace Laquila.Integrations.Worker.Clients.MasterData
{
    public interface IMasterDataClient
    {
        Task<MasterDataItemsPackageDTO?> GetItemsAsync(CancellationToken ct, int pageSize = 100);
        Task<MasterDataItemsPackageDTO?> SendItemsAsync(MasterDataItemsPackageDTO dto, Guid integrationId, CancellationToken ct);

        Task<MasterDataMandatorsDTO?> GetMandatorsAsync(CancellationToken ct, int pageSize = 100);
        Task<MasterDataMandatorsDTO?> SendMandatorsAsync(MasterDataMandatorsDTO dto, Guid integrationId, CancellationToken ct);
    }
}