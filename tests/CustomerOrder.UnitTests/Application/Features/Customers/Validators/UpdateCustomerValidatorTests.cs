using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Domain.ValueObjects;
using FluentValidation.TestHelper;
using static CustomerOrder.Application.Features.Customers.Commands.UpdateCustomer;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Validators;

[TestFixture]
public class UpdateCustomerValidatorTests
{
    private UpdateCustomerValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateCustomerValidator();
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Null()
    {
        var dto = new UpdateCustomerDto(null!, "John", "Doe", CreateAddress());

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Id);
    }

    [Test]
    public void Should_Have_Error_When_Id_Value_Is_LessThan_Or_EqualTo_Zero()
    {
        var customerId = CustomerId.Create(0);
        var dto = new UpdateCustomerDto(customerId, "John", "Doe", CreateAddress());

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Id.Id);
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "", "Doe", CreateAddress());

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "", CreateAddress());

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.LastName);
    }

    [Test]
    public void Should_Have_Error_When_Address_Is_Null()
    {
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", null!);

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Address);
    }

    [Test]
    public void Should_Have_Error_When_Street_Is_Empty()
    {
        var address = new Address("", "City", "State", "1000");
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", address);

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Address.Street);
    }

    [Test]
    public void Should_Have_Error_When_City_Is_Empty()
    {
        var address = new Address("Street", "", "State", "1000");
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", address);

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Address.City);
    }

    [Test]
    public void Should_Have_Error_When_State_Is_Empty()
    {
        var address = new Address("Street", "City", "", "1000");
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", address);

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Address.State);
    }

    [Test]
    public void Should_Have_Error_When_PostalCode_Is_Empty()
    {
        var address = new Address("Street", "City", "State", "");
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", address);

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCustomerDto.Address.PostalCode);
    }

    [Test]
    public void Should_Not_Have_Validation_Errors_For_Valid_Command()
    {
        var dto = new UpdateCustomerDto(CustomerId.Create(1), "John", "Doe", CreateAddress());

        var command = new UpdateCustomerCommand(dto);
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static Address CreateAddress()
    {
        return new Address("123 Main St", "Cityville", "Stateburg", "54321");
    }
}
