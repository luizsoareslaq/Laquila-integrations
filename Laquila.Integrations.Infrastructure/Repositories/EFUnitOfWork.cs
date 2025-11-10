using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Laquila.Integrations.Infrastructure.Repositories
{
    public class EfUnitOfWork(LaquilaHubContext db, Everest30Context everest30Db) : IUnitOfWork
    {
        private readonly LaquilaHubContext _db = db;
        private readonly Everest30Context _everest30Db = everest30Db;
        private IDbContextTransaction? _transaction;
        private IDbContextTransaction? _everestTransaction;

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



        public async Task BeginTransactionEverest30Async()
        {
            _everestTransaction = await _everest30Db.Database.BeginTransactionAsync();
        }

        public Task CommitEverest30Async()
        {
            return _everestTransaction?.CommitAsync() ?? Task.CompletedTask;
        }

        public Task RollbackEverest30Async()
        {
            return _everestTransaction?.RollbackAsync() ?? Task.CompletedTask;
        }

        public Task DetachEverest30EntitiesAsync<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                var entry = _everest30Db.Entry(entity);
                if (entry != null)
                    entry.State = EntityState.Detached;
            }

            return Task.CompletedTask;
        }
    }
}