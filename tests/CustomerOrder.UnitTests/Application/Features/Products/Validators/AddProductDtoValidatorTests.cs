using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;
using static CustomerOrder.Application.Features.Products.Commands.AddProduct;

namespace CustomerOrder.UnitTests.Application.Features.Products.Validators;

[TestFixture]
public class AddProductDtoValidatorTests
{
    private AddProductDtoValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddProductDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new AddProductDto("", Money.Create(10.0m, Currency.USD));
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddProductDto.Name);
    }

    [Test]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var longName = new string('A', 101);
        var dto = new AddProductDto(longName, Money.Create(10.0m, Currency.USD));
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddProductDto.Name)
              .WithErrorMessage("Name must not exceed 100 characters.");
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Null()
    {
        var dto = new AddProductDto("Valid Name", null!);
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddProductDto.Price)
              .WithErrorMessage("Price is required.");
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Zero()
    {
        var dto = new AddProductDto("Valid Name", Money.Create(0, Currency.USD));
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddProductDto.Price.Value)
              .WithErrorMessage("Price must be greater than zero.");
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        var dto = new AddProductDto("Valid Name", Money.Create(-5, Currency.USD));
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddProductDto.Price.Value)
              .WithErrorMessage("Price must be greater than zero.");
    }

    [Test]
    public void Should_Not_Have_Errors_For_Valid_Command()
    {
        var dto = new AddProductDto("Product Name", Money.Create(20.5m, Currency.USD));
        var command = new AddProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
