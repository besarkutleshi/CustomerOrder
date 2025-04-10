using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record OrderDto
{
    public OrderDto(DateTime orderDate, OrderStatus status, Money totalPrice)
    {
        OrderDate = orderDate;
        Status = status;
        TotalPrice = totalPrice;
    }

    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public Money TotalPrice { get; set; }
}
