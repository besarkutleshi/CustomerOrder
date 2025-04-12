using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Validators;

[TestFixture]
public class AddOrderItemValidatorTests
{
    private AddOrderItemValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new AddOrderItemValidator();
    }

    [Test]
    public void Should_Have_Error_When_ProductId_Is_Null()
    {
        var dto = new AddOrderItemDto(null!, 1);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Test]
    public void Should_Have_Error_When_ProductId_Is_Zero()
    {
        var dto = new AddOrderItemDto(ProductId.Create(0), 1);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ProductId.Id);
    }

    [Test]
    public void Should_Have_Error_When_Quantity_Is_Zero()
    {
        var dto = new AddOrderItemDto(ProductId.Create(1), 0);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Test]
    public void Should_Have_Error_When_Quantity_Is_Negative()
    {
        var dto = new AddOrderItemDto(ProductId.Create(1), -5);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Test]
    public void Should_Not_Have_Errors_When_Item_Is_Valid()
    {
        var dto = new AddOrderItemDto(ProductId.Create(5), 3);

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
