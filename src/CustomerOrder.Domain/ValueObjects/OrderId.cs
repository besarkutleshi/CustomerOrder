using CustomerOrder.Common.DDD;

namespace CustomerOrder.Domain.ValueObjects;
public class OrderId : ValueObject
{
    public int Id { get; set; }

    private OrderId(int id)
    {
        Id = id;
    }

    public static OrderId Create(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));

        return new OrderId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
