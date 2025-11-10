
namespace Laquila.Integrations.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task BeginTransactionEverest30Async();
        Task CommitEverest30Async();
        Task RollbackEverest30Async();
        Task DetachEverest30EntitiesAsync<T>(List<T> entities) where T : class;
    }
}