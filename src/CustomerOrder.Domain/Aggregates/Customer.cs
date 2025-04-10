using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Aggregates;
public class Customer : AggregateRoot<CustomerId>
{
    public Customer(string firstName, string lastName, string address, string postalCode)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;

    private readonly List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
}
