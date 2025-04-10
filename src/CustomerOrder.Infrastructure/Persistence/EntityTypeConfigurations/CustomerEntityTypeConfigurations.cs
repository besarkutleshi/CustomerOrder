using Microsoft.EntityFrameworkCore;
using CustomerOrder.Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerOrder.Domain.ValueObjects;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;

internal class CustomerEntityTypeConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => new CustomerId(id)
            );

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsMany(x => x.Orders, ordersBuilder => OrderEntityTypeConfigurations.Configure(ordersBuilder));
    }
}
