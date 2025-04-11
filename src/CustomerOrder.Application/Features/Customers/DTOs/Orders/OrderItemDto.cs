using CustomerOrder.Application.Features.Products.DTOs;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record OrderItemDto
{
    public OrderItemDto(ProductDto product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
}