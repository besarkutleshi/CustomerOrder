using CustomerOrder.Common.DDD;
using CustomerOrder.Domain.Enums;

namespace CustomerOrder.Domain.ValueObjects;
public class Money : ValueObject
{
    public decimal Value { get; set; }
    public Currency Currency { get; set; }

    public static Money Create(decimal value, Currency currency)
    {
        return new Money
        {
            Value = value,
            Currency = currency
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }
}
