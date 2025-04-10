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
                id => CustomerId.Create(id)
            );

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(c => c.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Street");

            addressBuilder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("City");

            addressBuilder.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("State");

            addressBuilder.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PostalCode");
        });

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsMany(x => x.Orders, ordersBuilder => OrderEntityTypeConfigurations.Configure(ordersBuilder));
    }
}
