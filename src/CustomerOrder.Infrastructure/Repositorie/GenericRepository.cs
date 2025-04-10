using CustomerOrder.Common;
using CustomerOrder.Common.DDD;
using CustomerOrder.Common.Extensions;
using CustomerOrder.Common.Response;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Infrastructure.Repositorie;
public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : AggregateRoot<TId> where TId : ValueObject
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<PaginatedResult<T>> GetAllPaginatedAsync(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        return await query
            .ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);
    }
}
