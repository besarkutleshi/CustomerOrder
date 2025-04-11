using CustomerOrder.Common.DDD;
using System.Text.Json.Serialization;

namespace CustomerOrder.Domain.ValueObjects;
public class OrderId : ValueObject
{
    public int Id { get; set; }

    [JsonConstructorAttribute]
    private OrderId(int id)
    {
        Id = id;
    }

    public static OrderId Create(int id)
    {
        return new OrderId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
