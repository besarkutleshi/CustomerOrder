using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Common.Helpers;
using CustomerOrder.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CustomerOrder.Application.Features.Products.Commands.AddProduct;
using static CustomerOrder.Application.Features.Products.Commands.DeleteProduct;
using static CustomerOrder.Application.Features.Products.Commands.UpdateProduct;
using static CustomerOrder.Application.Features.Products.Queries.GetProducts;

namespace CustomerOrder.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return ActionResponse.Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto, CancellationToken cancellationToken)
    {
        var command = new AddProductCommand(addProductDto);
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }

    [HttpDelete("{id}")]    
    public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(ProductId.Create(id));
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(updateProductDto);
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }
}
