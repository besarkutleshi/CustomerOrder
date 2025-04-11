using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Products.DTOs;
public record ProductDto
{
    public ProductDto(ProductId id, string name, Money price, bool isActive)
    {
        Id = id;
        Name = name;
        Price = price;
        IsActive = isActive;
    }

    public ProductId Id { get; set; }
    public string Name { get; set; }
    public Money Price { get; set; }
    public bool IsActive { get; set; }
}
