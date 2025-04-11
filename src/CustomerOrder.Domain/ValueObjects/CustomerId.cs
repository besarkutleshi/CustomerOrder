using CustomerOrder.Common.DDD;
using System.Text.Json.Serialization;

namespace CustomerOrder.Domain.ValueObjects;

public class CustomerId : ValueObject
{
    public int Id { get; set; }

    [JsonConstructorAttribute]
    private CustomerId(int id)
    {
        Id = id;
    }

    public static CustomerId Create(int id)
    {
        return new CustomerId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
