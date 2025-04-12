using CustomerOrder.API.Controllers;
using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static CustomerOrder.Application.Features.Products.Commands.AddProduct;
using static CustomerOrder.Application.Features.Products.Commands.DeleteProduct;
using static CustomerOrder.Application.Features.Products.Commands.UpdateProduct;
using static CustomerOrder.Application.Features.Products.Queries.GetProducts;

namespace CustomerOrder.UnitTests.API.Controllers;

[TestFixture]
public class ProductsControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private ProductsController _controller;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductsController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetProducts_ShouldSendQueryAndReturnResult()
    {
        List<ProductDto> productDtos =
        [
            new ProductDto(ProductId.Create(1), "Shoes", Money.Create(120, Currency.USD), true)
        ];
        var response = Result.Success(Success.Ok(productDtos));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetProducts(CancellationToken.None, 1, 10) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result!.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task AddProduct_ShouldSendCommandAndReturnResult()
    {
        var dto = new AddProductDto("Name", Money.Create(120, Currency.USD));
        var response = Result.Success(Success.Created(dto));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.AddProduct(dto, CancellationToken.None) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
        result!.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task DeleteProduct_ShouldSendCommandAndReturnResult()
    {
        int productId = 5;

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.NoContent()));

        var result = await _controller.DeleteProduct(productId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task UpdateProduct_ShouldSendCommandAndReturnResult()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "Shoes Updated", Money.Create(1, Currency.USD));
        var response = Result.Success(Success.Ok(dto));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.UpdateProduct(dto, CancellationToken.None) as ObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result!.Value.Should().BeEquivalentTo(response);
    }
}
