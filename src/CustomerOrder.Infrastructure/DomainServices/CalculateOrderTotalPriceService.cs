using CustomerOrder.Domain.Services;
using CustomerOrder.Domain.ValueObjects;
using CustomerOrder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Infrastructure.DomainServices;
internal class CalculateOrderTotalPriceService : ICalculateOrderTotalPriceService
{
    private readonly AppDbContext _appDbContext;

    public CalculateOrderTotalPriceService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<decimal> CalculateOrderTotalPrice(CustomerId customerId, OrderId orderId, CancellationToken cancellationToken)
    {
        var customer = await _appDbContext.Customers
            .AsNoTracking()
            .Include(x => x.Orders.Where(o => o.Id.Id == orderId.Id))
            //.Select(x => new Customer { })
            .FirstOrDefaultAsync(x => x.Id.Id == customerId.Id, cancellationToken) 
                ?? throw new ArgumentException($"There is no Customer with id: '{customerId.Id}'");
        
        var order = customer.Orders.FirstOrDefault() 
            ?? throw new ArgumentException($"There is no Order with id: '{orderId.Id}' for Customer with id: '{customerId.Id}'");

        var productIds = order.OrderItems.Select(x => x.ProductId).ToList();

        var products = await _appDbContext.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        decimal totalPrice = 0;

        // Calculate total price by multiplying price * quantity for each item
        foreach (var orderItem in order.OrderItems)
        {
            if (!products.TryGetValue(orderItem.ProductId, out var product))
            {
                throw new ArgumentException($"There is no Product with id: '{orderItem.ProductId}'");
            }

            totalPrice += product.Price.Value * orderItem.Quantity;
        }

        return totalPrice;
    }

}
