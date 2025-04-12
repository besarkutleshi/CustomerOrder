using CustomerOrder.Application.Features.Customers.Commands;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Net;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Commands;

[TestFixture]
public class DeleteCustomerCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private DeleteCustomer.DeleteCustomerCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Customer, CustomerId>())
                       .Returns(_customerRepoMock.Object);

        _handler = new DeleteCustomer.DeleteCustomerCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
    {
        var customerId = CustomerId.Create(1);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Customer?)null);

        var command = new DeleteCustomer.DeleteCustomerCommand(customerId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.Should().ContainSingle()
            .Which.Should().Contain("Customer with ID");
    }

    [Test]
    public async Task Handle_ShouldDeactivateCustomerAndReturnSuccess()
    {
        var customerId = CustomerId.Create(1);
        var customer = new Customer("Alice", "Smith", new Address("Street", "City", "State", "1000"));

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(customer);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

        var command = new DeleteCustomer.DeleteCustomerCommand(customerId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.Type.Should().Be(SuccessType.NoContent);

        _customerRepoMock.Verify(r => r.Update(It.Is<Customer>(c => !c.IsActive)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
