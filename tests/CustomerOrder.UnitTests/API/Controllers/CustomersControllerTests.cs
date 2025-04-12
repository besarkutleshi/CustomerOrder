using CustomerOrder.API.Controllers;
using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static CustomerOrder.Application.Features.Customers.Commands.AddCustomer;
using static CustomerOrder.Application.Features.Customers.Commands.AddOrder;
using static CustomerOrder.Application.Features.Customers.Commands.DeleteCustomer;
using static CustomerOrder.Application.Features.Customers.Commands.UpdateCustomer;
using static CustomerOrder.Application.Features.Customers.Queries.GetCustomerOrders;
using static CustomerOrder.Application.Features.Customers.Queries.GetCustomers;

namespace CustomerOrder.UnitTests.API.Controllers;

[TestFixture]
public class CustomersControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private CustomersController _controller;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CustomersController(_mediatorMock.Object);
    }

    [Test]
    public async Task AddCustomer_ShouldSendCommandAndReturnResult()
    {
        var dto = new AddCustomerDto("John", "Doe", new Address("Street 1", "City 1", "State 1", "PostalCode 1"));
        var command = new AddCustomerCommand(dto);
        var response = Result.Success(Success.Created(dto));
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.AddCustomer(dto, CancellationToken.None) as ObjectResult;

        result.Should().BeOfType<CreatedResult>();
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task DeleteCustomer_ShouldSendCommandAndReturnResult()
    {
        int customerId = 1;
        var expectedResponse = new { Deleted = true };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.NoContent()));

        var result = await _controller.DeleteCustomer(customerId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task UpdateCustomer_ShouldSendCommandAndReturnResult()
    {
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "Jane", "Smith", new Address("Street 1", "City 1", "State 1", "PostalCode 1"));
        var response = Result.Success(Success.Ok());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.UpdateCustomer(dto, CancellationToken.None) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task GetCustomers_ShouldSendQueryAndReturnResult()
    {
        List<CustomerDto> customerDtos =
        [
            new CustomerDto(CustomerId.Create(1),  "Jane", "Smith", new Address("Street 1", "City 1", "State 1", "PostalCode 1"), true),
            new CustomerDto(CustomerId.Create(2),  "Jane", "Smith", new Address("Street 1", "City 1", "State 1", "PostalCode 1"), true),
        ];
        var response = Result.Success(Success.Ok(customerDtos));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetCustomers(CancellationToken.None, 1, 10) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task GetCustomerOrders_ShouldSendQueryAndReturnResult()
    {
        var customerId = 1;
        List<OrderDto> orders =
        [
            new OrderDto(DateTime.Now, OrderStatus.Delivered, Money.Create(100, Currency.USD),
            [
                new OrderItemDto(new ProductDto(ProductId.Create(1), "Shoes", Money.Create(210, Currency.USD), true), 2)
            ])
        ];
        var response = Result.Success(Success.Ok(orders));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCustomerOrdersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetCustomerOrders(customerId, CancellationToken.None) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task AddOrder_ShouldSendCommandAndReturnResult()
    {
        int customerId = 1;
        var dto = new AddOrderDto(Money.Create(420, Currency.USD),
        [
            new AddOrderItemDto(ProductId.Create(1), 2),
        ]);
        var response = Result.Success(Success.Created(dto));
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.AddOrder(customerId, dto, CancellationToken.None) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
        result.Value.Should().BeEquivalentTo(response);
    }
}