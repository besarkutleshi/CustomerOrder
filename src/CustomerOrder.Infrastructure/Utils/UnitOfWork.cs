using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Common.DDD;
using CustomerOrder.Infrastructure.Persistence;
using CustomerOrder.Infrastructure.Repositorie;

namespace CustomerOrder.Infrastructure.Utils;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Dictionary<string, object> _repositories;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _repositories = [];
    }

    public IGenericRepository<T, TId> Repository<T, TId>() where T : AggregateRoot<TId> where TId : ValueObject
    {
        var entityType = typeof(T).Name;

        if (!_repositories.TryGetValue(entityType, out object? value))
        {
            var repositoryType = typeof(GenericRepository<T, TId>);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
            if (repositoryInstance == null)
                throw new ArgumentException("Cannot create instance of repository type.");
            value = repositoryInstance;
            _repositories.Add(entityType, value);
        }

        return (IGenericRepository<T, TId>)value;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
