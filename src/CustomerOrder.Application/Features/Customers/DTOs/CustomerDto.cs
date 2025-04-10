using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Customers.DTOs;
public record CustomerDto
{
    public CustomerDto(CustomerId id, string firstName, string lastName, Address address)
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
