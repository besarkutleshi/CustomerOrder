using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Aggregates;

public class Product : AggregateRoot<ProductId>
{
    private Product() { }

    public Product(string name, Money price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;

    public void Update(string name, Money price)
    {
        Name = name;
        Price = price;
    }
}
