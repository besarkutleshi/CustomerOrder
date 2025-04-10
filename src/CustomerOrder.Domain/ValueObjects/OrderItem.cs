using CustomerOrder.Common.DDD;

namespace CustomerOrder.Domain.ValueObjects;

public class OrderItem : ValueObject
{
    public OrderItem(ProductId productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public ProductId ProductId { get; set; } = null!;
    public int Quantity { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Quantity;
    }
}
