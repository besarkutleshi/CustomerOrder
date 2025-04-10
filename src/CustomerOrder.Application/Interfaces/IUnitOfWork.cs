using CustomerOrder.Common;
using CustomerOrder.Common.DDD;

namespace CustomerOrder.Application.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T, TId> Repository<T, TId>() where T : AggregateRoot<TId> where TId : ValueObject;
    Task<int> SaveAsync(CancellationToken cancellationToken);
}
