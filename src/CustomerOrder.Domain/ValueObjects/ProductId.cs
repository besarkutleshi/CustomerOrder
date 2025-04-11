using CustomerOrder.Common.DDD;
using System.Text.Json.Serialization;

namespace CustomerOrder.Domain.ValueObjects;
public class ProductId : ValueObject
{
    public int Id { get; set; }

    [JsonConstructorAttribute]
    private ProductId(int id)
    {
        Id = id;
    }

    public static ProductId Create(int id)
    {
        return new ProductId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
