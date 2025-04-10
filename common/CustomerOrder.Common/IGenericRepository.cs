using CustomerOrder.Common.DDD;
using CustomerOrder.Common.Response;

namespace CustomerOrder.Common;
public interface IGenericRepository<T, Tid> where T : AggregateRoot<Tid> where Tid : ValueObject
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<PaginatedResult<T>> GetAllPaginatedAsync(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
}

