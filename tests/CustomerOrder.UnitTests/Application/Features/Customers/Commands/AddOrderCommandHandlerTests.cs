using CustomerOrder.Application.Features.Customers.Commands;
using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Commands;

[TestFixture]
public class AddOrderCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private AddOrder.AddOrderCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Customer, CustomerId>())
                       .Returns(_customerRepoMock.Object);

        _handler = new AddOrder.AddOrderCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
    {
        var customerId = CustomerId.Create(1);
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Customer?)null);

        var dto = new AddOrderDto(Money.Create(99.99m, Currency.USD),
        [
            new AddOrderItemDto(ProductId.Create(10), 2)
        ]);

        var command = new AddOrder.AddOrderCommand(customerId, dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.First().Should().Contain("Customer with id");
    }

    [Test]
    public async Task Handle_ShouldAddOrderToCustomerAndReturnSuccess()
    {
        var customerId = CustomerId.Create(1);
        var dto = new AddOrderDto(Money.Create(150.00m, Currency.USD),
        [
            new AddOrderItemDto(ProductId.Create(101), 3),
            new AddOrderItemDto(ProductId.Create(102), 1)
        ]);

        var customer = new Customer("Jane", "Doe", new Address("Street", "City", "State", "0000"));

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(customer);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

        var command = new AddOrder.AddOrderCommand(customerId, dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.Result.Should().BeEquivalentTo(dto);

        _customerRepoMock.Verify(r => r.Update(customer), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
