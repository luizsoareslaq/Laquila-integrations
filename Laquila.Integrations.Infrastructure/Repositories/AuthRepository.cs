using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LaquilaHubContext _context;

        public AuthRepository(LaquilaHubContext context)
        {
            _context = context;
        }
        public async Task DeleteTokenAsync(LaqApiAuthTokens token)
        {
            _context.LaqApiAuthTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

        public async Task<LaqApiAuthTokens?> GetRefreshTokenAsync(string token)
        {
            return await _context.LaqApiAuthTokens
                            .FirstOrDefaultAsync(r => r.RefreshToken == token);
        }

        public async Task SaveTokenAsync(LaqApiAuthTokens token)
        {
            _context.LaqApiAuthTokens.Add(token);
            await _context.SaveChangesAsync();
        }
    }
}