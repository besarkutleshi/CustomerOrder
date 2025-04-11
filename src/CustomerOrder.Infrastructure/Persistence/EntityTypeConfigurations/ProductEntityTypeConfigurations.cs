using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;
internal class ProductEntityTypeConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => ProductId.Create(id)
            )
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(t => t.Price, priceBuilder => MoneyConfigurations.ConfigureMoney(priceBuilder));

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
