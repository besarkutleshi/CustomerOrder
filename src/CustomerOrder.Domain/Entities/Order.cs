using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Entities;
public class Order : Entity<OrderId>
{
    private Order() { }

    public DateTime OrderDate { get; private set; } = DateTime.Now;
    public OrderStatus Status { get; private set; } = OrderStatus.Processing;
    public Money TotalPrice { get; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _items.AsReadOnly();

    public Order(Money totalPrice, List<OrderItem> orderItems)
    {
        TotalPrice = totalPrice;
        _items = orderItems;
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }
}
