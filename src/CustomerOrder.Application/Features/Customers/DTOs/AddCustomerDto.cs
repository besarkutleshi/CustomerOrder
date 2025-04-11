using CustomerOrder.Domain.ValueObjects;
using FluentValidation;
using static CustomerOrder.Application.Features.Customers.Commands.AddCustomer;

namespace CustomerOrder.Application.Features.Customers.DTOs;

public record AddCustomerDto
{
    public AddCustomerDto(string firstName, string lastName, Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Address Address { get; set; } = null!;
}

public class AddCustomerValidator : AbstractValidator<AddCustomerCommand>
{
    public AddCustomerValidator()
    {
        RuleFor(x => x.AddCustomerDto.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.AddCustomerDto.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.AddCustomerDto.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.AddCustomerDto.Address.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");

        RuleFor(x => x.AddCustomerDto.Address.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        RuleFor(x => x.AddCustomerDto.Address.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

        RuleFor(x => x.AddCustomerDto.Address.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.");
    }
}