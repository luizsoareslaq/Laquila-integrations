using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Interfaces.Repositories.Everest30;
using Laquila.Integrations.Domain.Models.Everest30;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Laquila.Integrations.Infrastructure.Repositories.Everest30
{
    public class Everest30Repository : IEverest30Repository
    {
        private readonly Everest30Context _context;
        private readonly string lang = UserContext.Language ?? "en";

        public Everest30Repository(Everest30Context context)
        {
            _context = context;
        }
        public async Task<LoadOut> GetLoadOutByLoOe(long loOe)
        {
            var orders = await _context.LoadOut.Where(x => x.LoOe == loOe && x.LoMaCnpjOwner == UserContext.CompanyCnpj).FirstOrDefaultAsync()
            ?? throw new BadRequestException(MessageProvider.Get("OrderIdNotFound", lang));

            return orders;
        }

        public async Task<List<OrdersLine>> GetOeItemsByLoOe(long loOe)
        {
            var ordersLine = await _context.OrdersLine
                                        .Include(x => x.Orders)
                                           .ThenInclude(x => x.LoadOut)
                                        .Where(x => x.Orders.LoadOut.LoOe == loOe && x.Orders.LoadOut.LoMaCnpjOwner == UserContext.CompanyCnpj).ToListAsync()
                                                ?? throw new BadRequestException(MessageProvider.Get("OrderItemsNotFound", lang));

            return ordersLine;
        }
    }
}