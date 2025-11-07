using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models.Everest30;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Laquila.Integrations.Infrastructure.Repositories.Everest30
{
    public class Everest30Repository : IEverest30Repository
    {
        private readonly Everest30Context _context;
        public Everest30Repository(Everest30Context context)
        {
            _context = context;
        }
        public async Task<LoadOut> GetLoadOutByLoOe(long loOe)
        {
            var orders = await _context.LoadOut.Where(x => x.LoOe == loOe && x.LoMaCnpjOwner == UserContext.CompanyCnpj).FirstOrDefaultAsync()
            ?? throw new BadRequestException(MessageProvider.Get("OrderIdNotFound", UserContext.Language));

            return orders;
        }

        public async Task<List<OrdersLine>> GetOeItemsByLoOe(long loOe)
        {
            var ordersLine = await _context.OrdersLine
                                        .Include(x => x.Orders)
                                           .ThenInclude(x => x.LoadOut)
                                        .Where(x => x.Orders.LoadOut.LoOe == loOe && x.Orders.LoadOut.LoMaCnpjOwner == UserContext.CompanyCnpj).ToListAsync()
                                                ?? throw new BadRequestException(MessageProvider.Get("OrderItemsNotFound", UserContext.Language));

            return ordersLine;
        }
    }
}