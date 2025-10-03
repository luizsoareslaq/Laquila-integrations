using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Domain.Interfaces.Repositories
{
    public interface IEverest30Repository
    {
        Task<LoadOut> GetLoadOutByLoOe(long loOe, string companyCnpj);
        Task<List<OrdersLine>> GetOeItemsByLoOe(long loOe);
    }
}