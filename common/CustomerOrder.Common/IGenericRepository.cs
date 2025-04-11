using CustomerOrder.Common.DDD;
using CustomerOrder.Common.Response;
using System.Linq.Expressions;

namespace CustomerOrder.Common;
public interface IGenericRepository<T, TId> where T : AggregateRoot<TId> where TId : ValueObject
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<T, bool>>? predicate = null);
    Task<Dictionary<TId, T>> GetAllAsyncDict(CancellationToken cancellationToken, Expression<Func<T, bool>>? predicate = null);
    Task<PaginatedResult<T>> GetAllPaginatedAsync(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
}

