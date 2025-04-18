﻿using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Common.Helpers;
using CustomerOrder.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CustomerOrder.Application.Features.Customers.Commands.AddCustomer;
using static CustomerOrder.Application.Features.Customers.Commands.AddOrder;
using static CustomerOrder.Application.Features.Customers.Commands.DeleteCustomer;
using static CustomerOrder.Application.Features.Customers.Commands.UpdateCustomer;
using static CustomerOrder.Application.Features.Customers.Queries.GetCustomerOrders;
using static CustomerOrder.Application.Features.Customers.Queries.GetCustomers;

namespace CustomerOrder.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds a new customer.
    /// </summary>
    /// <param name="addCustomerDto">Add customer body</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Added customer body.</returns>
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDto addCustomerDto, CancellationToken cancellationToken)
    {
        var command = new AddCustomerCommand(addCustomerDto);
        var result = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No Content.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(CustomerId.Create(id));
        var result = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Updates a customer.
    /// </summary>
    /// <param name="updateCustomerDto">Update customer body</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Updated customer body.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto updateCustomerDto, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerCommand(updateCustomerDto);
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Gets the list of customers.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page Size</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of customer DTOs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetCustomersQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Gets the list of customer orders.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of order DTOs.</returns>
    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetCustomerOrders([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerOrdersQuery(CustomerId.Create(id));
        var result = await _mediator.Send(query, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Adds a new order to a customer.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="addOrderDto">Order body</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Added order body.</returns>
    [HttpPost("{id}/order")]
    public async Task<IActionResult> AddOrder([FromRoute] int id, [FromBody] AddOrderDto addOrderDto, CancellationToken cancellationToken)
    {
        var command = new AddOrderCommand(CustomerId.Create(id), addOrderDto);
        var result = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Response(result);
    }
}
