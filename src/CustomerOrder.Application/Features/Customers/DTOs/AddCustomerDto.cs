using CustomerOrder.Domain.ValueObjects;
using FluentValidation;

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

public class AddCustomerValidator : AbstractValidator<AddCustomerDto>
{
    public AddCustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(x => x.Address.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(x => x.Address.State)
            .NotEmpty()
            .WithMessage("State is required.");

        RuleFor(x => x.Address.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.");
    }
}