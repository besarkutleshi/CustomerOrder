using CustomerOrder.Domain.ValueObjects;
using FluentValidation;
using static CustomerOrder.Application.Features.Customers.Commands.UpdateCustomer;

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

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.UpdateCustomerDto.Id)
            .NotNull()
            .WithMessage("Customer ID is required.");

        RuleFor(x => x.UpdateCustomerDto.Id.Id)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0.")
            .When(x => x.UpdateCustomerDto.Id != null!);

        RuleFor(x => x.UpdateCustomerDto.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        RuleFor(x => x.UpdateCustomerDto.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(x => x.UpdateCustomerDto.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.UpdateCustomerDto.Address.Street)
            .NotEmpty()
            .WithMessage("Street is required.")
            .When(x => x.UpdateCustomerDto.Address != null!);

        RuleFor(x => x.UpdateCustomerDto.Address.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .When(x => x.UpdateCustomerDto.Address != null!);

        RuleFor(x => x.UpdateCustomerDto.Address.State)
            .NotEmpty()
            .WithMessage("State is required.")
            .When(x => x.UpdateCustomerDto.Address != null!);

        RuleFor(x => x.UpdateCustomerDto.Address.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .When(x => x.UpdateCustomerDto.Address != null!);
    }
}