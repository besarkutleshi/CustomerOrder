using CustomerOrder.Domain.ValueObjects;
using FluentValidation;

namespace CustomerOrder.Application.Features.Customers.DTOs;

public record UpdateCustomerDto
{
    public UpdateCustomerDto(CustomerId id, string firstName, string lastName, Address address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    public CustomerId Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Address Address { get; set; } = null!;
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Customer ID is required.");

        RuleFor(x => x.Id.Id)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0.");

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