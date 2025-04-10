using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.Enums;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Domain.Entities;
public class Order : Entity<OrderId>
{
    public DateTime OrderDate { get; private set; } = DateTime.Now;
    public OrderStatus Status { get; private set; } = OrderStatus.Processing;

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _items.AsReadOnly();

    public Order(List<OrderItem> orderItems)
    {
        _items = orderItems;
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }
}
