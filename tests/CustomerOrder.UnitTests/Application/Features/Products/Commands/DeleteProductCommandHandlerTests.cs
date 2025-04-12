using CustomerOrder.Application.Features.Products.Commands;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Net;

namespace CustomerOrder.UnitTests.Application.Features.Products.Commands;

[TestFixture]
public class DeleteProductCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Product, ProductId>> _productRepoMock;
    private DeleteProduct.DeleteProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IGenericRepository<Product, ProductId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Product, ProductId>())
                       .Returns(_productRepoMock.Object);

        _handler = new DeleteProduct.DeleteProductCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnValidationError_WhenProductIdIsInvalid()
    {
        var invalidId = ProductId.Create(0);
        var command = new DeleteProduct.DeleteProductCommand(invalidId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("Validation");
        result.Error.ErrorMessages.Should().Contain("Product Id should be greater than 0.");
    }

    [Test]
    public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var productId = ProductId.Create(1);

        _productRepoMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var command = new DeleteProduct.DeleteProductCommand(productId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.Single().Should().Contain($"Product with id: '{productId.Id}' not found");
    }

    [Test]
    public async Task Handle_ShouldDeactivateProduct_AndReturnSuccess()
    {
        var productId = ProductId.Create(2);
        var product = new Product("Product A", Money.Create(100, Currency.USD));

        _productRepoMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(product);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

        var command = new DeleteProduct.DeleteProductCommand(productId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.Type.Should().Be(SuccessType.NoContent);

        _productRepoMock.Verify(r => r.Update(It.Is<Product>(p => !p.IsActive)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
