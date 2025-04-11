using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Products.DTOs;
public record UpdateProductDto
{
    public UpdateProductDto(ProductId id, string name, Money price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public ProductId Id { get; set; }
    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
}
