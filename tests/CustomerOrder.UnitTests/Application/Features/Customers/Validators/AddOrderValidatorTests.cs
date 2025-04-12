using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Validators;

[TestFixture]
public class AddOrderValidatorTests
{
    private AddOrderValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new AddOrderValidator();
    }

    [Test]
    public void Should_Have_Error_When_TotalPrice_Is_Null()
    {
        var dto = new AddOrderDto(null!, CreateValidItems());

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Test]
    public void Should_Have_Error_When_TotalPrice_Is_Zero()
    {
        var dto = new AddOrderDto(Money.Create(0, Currency.EUR), CreateValidItems());

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.TotalPrice.Value);
    }

    [Test]
    public void Should_Have_Error_When_Items_List_Is_Empty()
    {
        var dto = new AddOrderDto(Money.Create(10, Currency.USD), []);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Test]
    public void Should_Have_Error_When_Items_Is_Null()
    {
        var dto = new AddOrderDto(Money.Create(10, Currency.USD), null!);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Test]
    public void Should_Not_Have_Errors_When_Dto_Is_Valid()
    {
        var dto = new AddOrderDto(Money.Create(99.99m, Currency.USD), CreateValidItems());

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static List<AddOrderItemDto> CreateValidItems()
    {
        return
        [
            new AddOrderItemDto(ProductId.Create(1), 2)
        ];
    }
}
