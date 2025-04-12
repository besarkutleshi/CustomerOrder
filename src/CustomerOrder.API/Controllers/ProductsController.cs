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

    /// <summary>
    /// Gets the list of products.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page Size</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>List of products DTOs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="addProductDto">Product body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Added product body.</returns>
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto, CancellationToken cancellationToken)
    {
        var command = new AddProductCommand(addProductDto);
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No Content.</returns>
    [HttpDelete("{id}")]    
    public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(ProductId.Create(id));
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }

    /// <summary>
    /// Updates a product
    /// </summary>
    /// <param name="updateProductDto">Update product body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Updated product body.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(updateProductDto);
        var result = await _mediator.Send(command, cancellationToken);
        return ActionResponse.Response(result);
    }
}
