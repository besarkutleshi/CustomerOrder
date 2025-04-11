using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Products.DTOs;
public record AddProductDto
{
    public AddProductDto(string name, Money price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
}
