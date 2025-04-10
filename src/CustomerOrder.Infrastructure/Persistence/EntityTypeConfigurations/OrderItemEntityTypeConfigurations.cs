using CustomerOrder.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;
internal class OrderItemEntityTypeConfigurations
{
    public static void Configure<T>(OwnedNavigationBuilder<T, OrderItem> orderItemsBuilder) where T : class
    {
        orderItemsBuilder.ToTable("OrderItems");

        orderItemsBuilder.Property(x => x.Quantity)
            .IsRequired();

        orderItemsBuilder.Property(x => x.ProductId)
            .IsRequired();
    }
}
