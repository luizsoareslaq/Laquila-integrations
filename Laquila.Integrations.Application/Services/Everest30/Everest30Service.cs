using Laquila.Integrations.Application.Interfaces.Everest30;
using Laquila.Integrations.Domain.Interfaces.Repositories.Everest30;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Application.Services.Everest30
{
    public class Everest30Service : IEverest30Service
    {
        private readonly IEverest30Repository _everest30Repository;
        public Everest30Service(IEverest30Repository everest30Repository)
        {
            _everest30Repository = everest30Repository;
        }

        public async Task<LoadOut> GetLoadOutByLoOe(long loOe)
        {
            return await _everest30Repository.GetLoadOutByLoOe(loOe);
        }

        public async Task<List<OrdersLine>> GetOeItemsByLoOe(long loOe)
        {
            return await _everest30Repository.GetOeItemsByLoOe(loOe);
        }
    }
}