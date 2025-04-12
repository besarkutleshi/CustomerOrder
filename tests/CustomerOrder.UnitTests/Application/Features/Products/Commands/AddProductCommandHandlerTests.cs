using CustomerOrder.Application.Features.Products.Commands;
using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Products.Commands;

[TestFixture]
public class AddProductCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Product, ProductId>> _productRepoMock;
    private AddProduct.AddProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IGenericRepository<Product, ProductId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Product, ProductId>())
                       .Returns(_productRepoMock.Object);

        _handler = new AddProduct.AddProductCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldAddProductAndReturnSuccessResult()
    {
        var dto = new AddProductDto("Test Product", Money.Create(49.99m, Currency.USD));
        var command = new AddProduct.AddProductCommand(dto);

        _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Response.Result.Should().BeEquivalentTo(dto);

        _productRepoMock.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.Name == dto.Name &&
            p.Price == dto.Price
        ), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
