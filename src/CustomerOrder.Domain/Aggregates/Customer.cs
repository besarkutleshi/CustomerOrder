using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Aggregates;
public class Customer : AggregateRoot<CustomerId>
{
    private Customer() { }

    public Customer(string firstName, string lastName, Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    // optimize for EF Core
    public Customer(List<Order> orders)
    {
        _orders = orders;
    }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Address Address { get; private set; } = null!;

    private readonly List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public void Update(string firstName, string lastName, Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }
}
