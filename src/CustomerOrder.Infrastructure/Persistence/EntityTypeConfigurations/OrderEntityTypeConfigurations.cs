﻿using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;

internal class OrderEntityTypeConfigurations
{
    public static void Configure<T>(OwnedNavigationBuilder<T, Order> ordersBuilder) where T : class
    {
        ordersBuilder.ToTable("Orders");
        ordersBuilder.HasKey("Id", "CustomerId");

        ordersBuilder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => OrderId.Create(id)
            )
            .ValueGeneratedOnAdd();

        ordersBuilder.WithOwner().HasForeignKey("CustomerId");

        ordersBuilder.Property(o => o.OrderDate)
            .IsRequired();

        ordersBuilder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        ordersBuilder.OwnsOne(x => x.TotalPrice, priceBuilder => MoneyConfigurations.ConfigureMoney(priceBuilder, "TotalPrice", "Currency"));

        ordersBuilder.OwnsMany(ordersBuilder => ordersBuilder.OrderItems, orderItemsBuilder => OrderItemEntityTypeConfigurations.Configure(orderItemsBuilder));

        ordersBuilder.Navigation(x => x.OrderItems).Metadata.SetField("_items");
        ordersBuilder.Navigation(x => x.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
