using CustomerOrder.Application.Features.Customers.Commands;
using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Net;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Commands;

[TestFixture]
public class UpdateCustomerCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private UpdateCustomer.UpdateCustomerCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Customer, CustomerId>())
                       .Returns(_customerRepoMock.Object);

        _handler = new UpdateCustomer.UpdateCustomerCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
    {
        var customerId = CustomerId.Create(1);
        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Customer?)null);

        var dto = new UpdateCustomerDto(customerId, "John", "Doe", new Address("Street", "City", "State", "12345"));
        var command = new UpdateCustomer.UpdateCustomerCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.Should().ContainSingle()
            .Which.Should().Contain($"Customer with ID {customerId.Id} not found");
    }

    [Test]
    public async Task Handle_ShouldUpdateCustomerAndReturnSuccess()
    {
        var customerId = CustomerId.Create(1);
        var existingCustomer = new Customer("OldFirst", "OldLast", new Address("OldSt", "OldCity", "OldState", "1111"));

        var newAddress = new Address("New St", "New City", "New State", "9999");
        var dto = new UpdateCustomerDto(customerId, "Jane", "Smith", newAddress);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(existingCustomer);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

        var command = new UpdateCustomer.UpdateCustomerCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.GetHttpStatusCodeBySuccessType()
            .Should().Be(HttpStatusCode.NoContent);

        _customerRepoMock.Verify(r => r.Update(It.Is<Customer>(c =>
            c.FirstName == "Jane" &&
            c.LastName == "Smith" &&
            c.Address == newAddress)), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
