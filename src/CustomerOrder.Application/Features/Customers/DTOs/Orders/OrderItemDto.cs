using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record OrderItemDto
{
    public OrderItemDto(ProductId productId, string productName, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
    }

    public ProductId ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}