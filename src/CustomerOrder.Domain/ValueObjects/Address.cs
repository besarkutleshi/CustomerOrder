using CustomerOrder.Common.DDD;

namespace CustomerOrder.Domain.ValueObjects;
public class Address : ValueObject
{
    public Address(string street, string city, string state, string postalCode)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }

    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return PostalCode;
    }
}
