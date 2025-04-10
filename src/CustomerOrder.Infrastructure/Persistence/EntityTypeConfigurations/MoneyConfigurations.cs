using CustomerOrder.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrder.Infrastructure.Persistence.EntityTypeConfigurations;

internal class MoneyConfigurations
{
    public static void ConfigureMoney<T>(OwnedNavigationBuilder<T, Money> builder, string priceColumnName = "Price", string currencyColumnName = "Currency") where T : class
    {
        builder.WithOwner();

        builder.Property(t => t.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName(priceColumnName);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName(currencyColumnName)
            .HasMaxLength(20);
    }
}