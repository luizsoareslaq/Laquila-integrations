using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Domain.Interfaces.Repositories.Everest30
{
    public interface IMasterDataRepository
    {
        Task<List<Article>> GetItemsByAtIdAsync(List<string> atIds);
        Task InsertItemsAsync(List<Article> items);
        Task UpdateItemsAsync(List<Article> items);

        Task<List<Mandator>> GetMandatorsByMaCodeAsync(List<long> maCodes);
        Task InsertMandatorsAsync(List<Mandator> mandators);
        Task UpdateMandatorsAsync(List<Mandator> mandators);
    }
}