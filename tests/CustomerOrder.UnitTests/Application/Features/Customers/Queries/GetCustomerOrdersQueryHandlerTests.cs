using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Application.Features.Customers.Queries;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Queries;

[TestFixture]
public class GetCustomerOrdersQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private Mock<IGenericRepository<Product, ProductId>> _productRepoMock;
    private GetCustomerOrders.GetCustomerOrdersQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();
        _productRepoMock = new Mock<IGenericRepository<Product, ProductId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Customer, CustomerId>())
                       .Returns(_customerRepoMock.Object);

        _unitOfWorkMock.Setup(u => u.Repository<Product, ProductId>())
                       .Returns(_productRepoMock.Object);

        _handler = new GetCustomerOrders.GetCustomerOrdersQueryHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
    {
        var customerId = CustomerId.Create(1);
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Customer?)null);

        var command = new GetCustomerOrders.GetCustomerOrdersQuery(customerId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.Single().Should().Contain($"Customer with id: '{customerId.Id}' not found.");
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyOrders_WhenCustomerHasNoOrders()
    {
        var customerId = CustomerId.Create(2);
        var customer = new Customer("John", "Doe", new Address("Street", "City", "State", "1000"));

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(customer);

        var query = new GetCustomerOrders.GetCustomerOrdersQuery(customerId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.Result.Should().BeOfType<List<OrderDto>>();
        ((List<OrderDto>)result.Response.Result!).Should().BeEmpty();
    }

    [Test]
    public async Task Handle_ShouldReturnOrders_WhenCustomerHasOrders()
    {
        var customerId = CustomerId.Create(3);

        var productId1 = ProductId.Create(100);
        var productId2 = ProductId.Create(200);

        List<ProductId> productIds = [productId1, productId2];

        var orderItems = new List<OrderItem>
        {
            new(productId1, 1),
            new(productId2, 2)
        };

        var order = new Order(Money.Create(99.99m, Currency.USD), orderItems);

        var customer = new Customer("Jane", "Smith", new Address("Main", "City", "State", "2000"));
        customer.AddOrder(order);

        var product1 = new Product("Product 1", Money.Create(10.0m, Currency.USD));
        var product2 = new Product("Product 2", Money.Create(20.0m, Currency.USD));

        var productDict = new Dictionary<ProductId, Product>
        {
            [productId1] = product1,
            [productId2] = product2
        };

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(customer);

        _productRepoMock.Setup(r => r.GetAllAsyncDict(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
                        .ReturnsAsync(productDict);

        var query = new GetCustomerOrders.GetCustomerOrdersQuery(customerId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var orderDtos = result.Response.Result.Should().BeOfType<List<OrderDto>>().Subject;

        orderDtos.Should().HaveCount(1);
        orderDtos[0].Items.Should().HaveCount(2);
        orderDtos[0].Items[0].Product.Name.Should().Be("Product 1");
        orderDtos[0].Items[1].Product.Name.Should().Be("Product 2");
    }
}
