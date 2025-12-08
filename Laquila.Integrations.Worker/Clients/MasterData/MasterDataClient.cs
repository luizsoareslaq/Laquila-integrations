using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Worker.Infrastructure.Http;

namespace Laquila.Integrations.Worker.Clients.MasterData
{
    public class MasterDataClient : IMasterDataClient
    {
        private readonly IEverestHttpClient _http;

        public MasterDataClient(IEverestHttpClient http)
        {
            _http = http;
        }

        public Task<MasterDataItemsPackageDTO?> GetItemsAsync(CancellationToken ct, int pageSize = 100)
        {
            return _http.GetAsync<MasterDataItemsPackageDTO>(
                "/masterdata/items",
                new { pageSize },
                ct
            );
        }

        public Task<MasterDataItemsPackageDTO?> SendItemsAsync(MasterDataItemsPackageDTO dto, Guid integrationId, CancellationToken ct)
        {
            return _http.PostAsync<MasterDataItemsPackageDTO>(
                $"/masterdata/items/{integrationId}",
                dto,
                ct
            );
        }

        public Task<MasterDataMandatorsDTO?> GetMandatorsAsync(CancellationToken ct, int pageSize = 100)
        {
            return _http.GetAsync<MasterDataMandatorsDTO>(
                "/masterdata/mandators",
                new { pageSize },
                ct
            );
        }

        public Task<MasterDataMandatorsDTO?> SendMandatorsAsync(MasterDataMandatorsDTO dto, Guid integrationId, CancellationToken ct)
        {
            return _http.PostAsync<MasterDataMandatorsDTO>(
                $"/masterdata/mandators/{integrationId}",
                dto,
                ct
            );
        }
    }
}