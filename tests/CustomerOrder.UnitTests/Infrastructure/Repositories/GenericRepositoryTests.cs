using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using CustomerOrder.Infrastructure.Persistence;
using CustomerOrder.Infrastructure.Repositorie;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.UnitTests.Infrastructure.Repositories;

[TestFixture]
public class GenericRepositoryTests
{
    private AppDbContext _context = null!;
    private GenericRepository<Product, ProductId> _repository = null!;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new GenericRepository<Product, ProductId>(_context);
    }

    [Test]
    public async Task AddAsync_ShouldAddEntity()
    {
        var entity = new Product("Shoes", Money.Create(100, Domain.Enums.Currency.USD));

        await _repository.AddAsync(entity, _cancellationToken);
        await _context.SaveChangesAsync(_cancellationToken);

        var result = await _context.Products.FirstOrDefaultAsync(x => x.Id == entity.Id, _cancellationToken);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Shoes");
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        var entity = new Product("Shoes Two", Money.Create(100, Domain.Enums.Currency.USD));
        await _context.Products.AddAsync(entity, _cancellationToken);
        await _context.SaveChangesAsync(_cancellationToken);

        var result = await _repository.GetByIdAsync(entity.Id, _cancellationToken);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Shoes Two");
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        await _context.Products.AddRangeAsync(
            new Product("Shoes 1", Money.Create(100, Domain.Enums.Currency.USD)),
            new Product("Shoes 2", Money.Create(100, Domain.Enums.Currency.USD)),
            new Product("Shoes 3", Money.Create(100, Domain.Enums.Currency.USD))
        );
        await _context.SaveChangesAsync(_cancellationToken);

        var result = await _repository.GetAllAsync(_cancellationToken);

        result.Should().HaveCount(3);
    }

    [Test]
    public async Task GetAllAsync_WithPredicate_ShouldFilterCorrectly()
    {
        await _context.Products.AddRangeAsync(
            new Product("First", Money.Create(100, Domain.Enums.Currency.USD)),
            new Product("First", Money.Create(100, Domain.Enums.Currency.USD)),
            new Product("Shoes 3", Money.Create(100, Domain.Enums.Currency.USD))
        );
        await _context.SaveChangesAsync(_cancellationToken);

        var result = await _repository.GetAllAsync(_cancellationToken, x => x.Name == "First");

        result.Should().HaveCount(2);
        result.All(x => x.Name == "First").Should().BeTrue();
    }

    [Test]
    public async Task GetAllPaginatedAsync_ShouldReturnCorrectPage()
    {
        for (int i = 1; i <= 25; i++)
        {
            await _context.Products.AddAsync(new Product($"Entity {i}", Money.Create(i, Domain.Enums.Currency.USD)), _cancellationToken);
        }
        await _context.SaveChangesAsync(_cancellationToken);

        var pageResult = await _repository.GetAllPaginatedAsync(_cancellationToken, pageNumber: 2, pageSize: 10);

        pageResult.Data.Should().HaveCount(10);
        pageResult.PageNumber.Should().Be(2);
        pageResult.TotalRecords.Should().Be(25);
    }

    [Test]
    public void Update_ShouldTrackModifiedEntity()
    {
        var entity = new Product("First", Money.Create(100, Domain.Enums.Currency.USD));
        _context.Products.Add(entity);
        _context.SaveChanges();

        entity.Update("Updated Name", entity.Price);
        _repository.Update(entity);
        _context.SaveChanges();

        _context.Products.First(e => e.Id == entity.Id).Name.Should().Be("Updated Name");
    }

    [Test]
    public async Task Delete_ShouldRemoveEntity()
    {
        var entity = new Product("First", Money.Create(100, Domain.Enums.Currency.USD));
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync(_cancellationToken);

        _repository.Delete(entity);
        await _context.SaveChangesAsync(_cancellationToken);

        var exists = await _context.Products.AnyAsync(x => x.Id == entity.Id);
        exists.Should().BeFalse();
    }
}
