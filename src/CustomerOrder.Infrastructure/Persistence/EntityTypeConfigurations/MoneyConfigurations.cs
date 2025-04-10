using CustomerOrder.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;

internal class MoneyConfigurations
{
    public static void ConfigureMoney<T>(OwnedNavigationBuilder<T, Money> builder) where T : class
    {
        builder.WithOwner();

        builder.Property(t => t.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("Price");

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("Currency")
            .HasMaxLength(20);
    }
}