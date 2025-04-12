using CustomerOrder.Application.Features.Products.Commands;
using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Products.Commands;

[TestFixture]
public class UpdateProductCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Product, ProductId>> _productRepoMock;
    private UpdateProduct.UpdateProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IGenericRepository<Product, ProductId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Product, ProductId>())
                       .Returns(_productRepoMock.Object);

        _handler = new UpdateProduct.UpdateProductCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var productId = ProductId.Create(1);
        var dto = new UpdateProductDto(productId, "Updated Name", Money.Create(99.99m, Domain.Enums.Currency.USD));
        var command = new UpdateProduct.UpdateProductCommand(dto);

        _productRepoMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("NotFound");
        result.Error.ErrorMessages.Single().Should().Contain($"Product with id: '{productId.Id}' not found");
    }

    [Test]
    public async Task Handle_ShouldUpdateProductAndReturnSuccess()
    {
        var productId = ProductId.Create(2);
        var existingProduct = new Product("Old Name", Money.Create(20, Domain.Enums.Currency.USD));

        var updatedDto = new UpdateProductDto(productId, "New Product", Money.Create(45, Domain.Enums.Currency.USD));
        var command = new UpdateProduct.UpdateProductCommand(updatedDto);

        _productRepoMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _unitOfWorkMock
            .Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _productRepoMock.Verify(r => r.Update(It.Is<Product>(p =>
            p.Name == updatedDto.Name &&
            p.Price == updatedDto.Price
        )), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
