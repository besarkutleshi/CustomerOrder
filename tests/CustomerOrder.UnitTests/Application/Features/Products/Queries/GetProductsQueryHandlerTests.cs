using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Features.Products.Queries;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Products.Queries;

[TestFixture]
public class GetProductsQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Product, ProductId>> _productRepoMock;
    private GetProducts.GetProductQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IGenericRepository<Product, ProductId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Product, ProductId>())
                       .Returns(_productRepoMock.Object);

        _handler = new GetProducts.GetProductQueryHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var emptyResult = new PaginatedResult<Product>(
            data: [],
            totalRecords: 0,
            pageNumber: 1,
            pageSize: 10
        );

        _productRepoMock.Setup(r =>
            r.GetAllPaginatedAsync(It.IsAny<CancellationToken>(), 1, 10))
            .ReturnsAsync(emptyResult);

        var query = new GetProducts.GetProductsQuery(1, 10);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var paginated = result.Response.Result.Should().BeOfType<PaginatedResult<ProductDto>>().Subject;
        paginated.TotalRecords.Should().Be(0);
        paginated.Data.Should().BeEmpty();
    }

    [Test]
    public async Task Handle_ShouldReturnPaginatedProductDtos_WhenProductsExist()
    {
        var products = new List<Product>
        {
            new("Product A", Money.Create(10.0m, Domain.Enums.Currency.USD)),
            new("Product B", Money.Create(20.0m, Domain.Enums.Currency.USD))
        };

        var paginatedResult = new PaginatedResult<Product>(
            data: products,
            totalRecords: 2,
            pageNumber: 1,
            pageSize: 10
        );

        _productRepoMock.Setup(r =>
            r.GetAllPaginatedAsync(It.IsAny<CancellationToken>(), 1, 10))
            .ReturnsAsync(paginatedResult);

        var query = new GetProducts.GetProductsQuery(1, 10);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var paginated = result.Response.Result.Should().BeOfType<PaginatedResult<ProductDto>>().Subject;

        paginated.TotalRecords.Should().Be(2);
        paginated.Data.Should().HaveCount(2);
        paginated.Data[0].Name.Should().Be("Product A");
        paginated.Data[1].Name.Should().Be("Product B");
    }
}
