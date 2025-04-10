namespace CustomerOrder.Domain.ValueObjects;
public class ProductId
{
    public int Id { get; set; }

    private ProductId(int id)
    {
        Id = id;
    }

    public static ProductId Create(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Product ID must be greater than zero.", nameof(id));

        return new ProductId(id);
    }
}
