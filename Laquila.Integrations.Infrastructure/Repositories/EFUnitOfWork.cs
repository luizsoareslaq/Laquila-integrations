using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class EfUnitOfWork(LaquilaHubContext db) : IUnitOfWork
    {
        private readonly LaquilaHubContext _db = db;
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
            return _transaction?.CommitAsync() ?? Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            return _transaction?.RollbackAsync() ?? Task.CompletedTask;
        }
    }
}