using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Aggregates;

public class Product : AggregateRoot<ProductId>
{
    private Product() { }

    public Product(ProductId id, string name, Money price)
        : base(id)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
}
