using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record OrderDto
{
    public OrderDto(DateTime orderDate, OrderStatus status, Money totalPrice, List<OrderItemDto> items)
    {
        OrderDate = orderDate;
        Status = status;
        TotalPrice = totalPrice;
        Items = items;
    }

    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public Money TotalPrice { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
