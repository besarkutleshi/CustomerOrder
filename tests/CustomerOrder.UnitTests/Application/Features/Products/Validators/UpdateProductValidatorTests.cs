using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;
using static CustomerOrder.Application.Features.Products.Commands.UpdateProduct;

namespace CustomerOrder.UnitTests.Application.Features.Products.Validators;

[TestFixture]
public class UpdateProductValidatorTests
{
    private UpdateProductValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateProductValidator();
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Null()
    {
        var dto = new UpdateProductDto(null!, "Product A", Money.Create(10, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Id);
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Less_Than_One()
    {
        var dto = new UpdateProductDto(ProductId.Create(0), "Product A", Money.Create(10, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Id.Id)
              .WithErrorMessage("Id must be greaeter than 0.");
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "", Money.Create(10, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Name);
    }

    [Test]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var longName = new string('X', 101);
        var dto = new UpdateProductDto(ProductId.Create(1), longName, Money.Create(10, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Name)
              .WithErrorMessage("Name must not exceed 100 characters.");
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Null()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "Product A", null!);
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Price);
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Zero()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "Product A", Money.Create(0, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Price.Value)
              .WithErrorMessage("Price must be greater than 0.");
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "Product A", Money.Create(-5, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateProductDto.Price.Value)
              .WithErrorMessage("Price must be greater than 0.");
    }

    [Test]
    public void Should_Not_Have_ValidationErrors_For_Valid_Command()
    {
        var dto = new UpdateProductDto(ProductId.Create(1), "Valid Product", Money.Create(20.99m, Currency.USD));
        var command = new UpdateProductCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
