using CustomerOrder.Common.DDD;

namespace CustomerOrder.Domain.ValueObjects;

public class CustomerId : ValueObject
{
    public int Id { get; set; }

    private CustomerId(int id)
    {
        Id = id;
    }

    public static CustomerId Create(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));

        return new CustomerId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
