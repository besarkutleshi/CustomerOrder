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

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Address Address { get; private set; } = null!;

    private List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public void Update(string firstName, string lastName, Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    public void AddOrder(Order order)
    {
        _orders ??= [];
        _orders.Add(order);
    }
}
