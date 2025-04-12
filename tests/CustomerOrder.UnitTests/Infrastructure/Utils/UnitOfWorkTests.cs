using CustomerOrder.Application.Interfaces;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using CustomerOrder.Infrastructure.Persistence;
using CustomerOrder.Infrastructure.Repositorie;
using CustomerOrder.Infrastructure.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.UnitTests.Infrastructure.Utils;

[TestFixture]
public class UnitOfWorkTests
{
    private AppDbContext _context = null!;
    private IUnitOfWork _unitOfWork = null!;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _unitOfWork.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _unitOfWork = new UnitOfWork(_context);
    }

    [Test]
    public void Repository_ShouldReturnGenericRepositoryInstance()
    {
        var repo = _unitOfWork.Repository<Product, ProductId>();

        repo.Should().NotBeNull();
        repo.Should().BeOfType<GenericRepository<Product, ProductId>>();
    }

    [Test]
    public void Repository_ShouldCacheRepositoryInstance()
    {
        var repo1 = _unitOfWork.Repository<Product, ProductId>();
        var repo2 = _unitOfWork.Repository<Product, ProductId>();

        repo1.Should().BeSameAs(repo2);
    }

    [Test]
    public async Task SaveAsync_ShouldCallDbContextSaveChanges()
    {
        var entity = new Product("Shoes", Money.Create(100, Domain.Enums.Currency.USD));
        await _context.Products.AddAsync(entity);

        await _unitOfWork.SaveAsync(CancellationToken.None);

        var result = await _context.Products.FirstOrDefaultAsync(x => x.Id == entity.Id, _cancellationToken);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Shoes");
    }
}
