using CustomerOrder.Domain.ValueObjects;
namespace CustomerOrder.Domain.Services;

public interface ICalculateOrderTotalPriceService
{
    Task<decimal> CalculateOrderTotalPrice(CustomerId customerId, OrderId orderId, CancellationToken cancellationToken);
}
