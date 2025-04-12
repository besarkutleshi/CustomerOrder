using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;
using static CustomerOrder.Application.Features.Customers.Commands.AddCustomer;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Validators;

[TestFixture]
public class AddCustomerValidatorTests
{
    private AddCustomerValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new AddCustomerValidator();
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = CreateValidDto();
        dto.FirstName = "";

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var dto = CreateValidDto();
        dto.LastName = "";

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.LastName);
    }

    [Test]
    public void Should_Have_Error_When_Street_Is_Empty()
    {
        var address = new Address("", "City", "State", "12345");
        var dto = new AddCustomerDto("John", "Doe", address);

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.Address.Street);
    }

    [Test]
    public void Should_Have_Error_When_City_Is_Empty()
    {
        var address = new Address("Main Street", "", "State", "12345");
        var dto = new AddCustomerDto("John", "Doe", address);

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.Address.City);
    }

    [Test]
    public void Should_Have_Error_When_State_Is_Empty()
    {
        var address = new Address("Main Street", "City", "", "12345");
        var dto = new AddCustomerDto("John", "Doe", address);

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.Address.State);
    }

    [Test]
    public void Should_Have_Error_When_PostalCode_Is_Empty()
    {
        var address = new Address("Main Street", "City", "State", "");
        var dto = new AddCustomerDto("John", "Doe", address);

        var command = new AddCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AddCustomerDto.Address.PostalCode);
    }

    [Test]
    public void Should_Not_Have_Errors_For_Valid_Command()
    {
        var dto = CreateValidDto();
        var command = new AddCustomerCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static AddCustomerDto CreateValidDto()
    {
        return new AddCustomerDto(
            "John",
            "Doe",
            new Address("Main Street", "CityName", "StateName", "12345")
        );
    }
}
