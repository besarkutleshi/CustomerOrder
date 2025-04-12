using CustomerOrder.Application.Features.Customers.Commands;
using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Commands;

[TestFixture]
public class AddCustomerCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private AddCustomer.AddCustomerCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();

        _unitOfWorkMock
            .Setup(u => u.Repository<Customer, CustomerId>())
            .Returns(_customerRepoMock.Object);

        _handler = new AddCustomer.AddCustomerCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldAddCustomerAndReturnSuccessResult()
    {
        var address = new Address("Street 1", "City A", "State B", "12345");
        var dto = new AddCustomerDto("John", "Doe", address);
        var command = new AddCustomer.AddCustomerCommand(dto);

        _customerRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Response.Result.Should().BeEquivalentTo(dto);

        _customerRepoMock.Verify(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}