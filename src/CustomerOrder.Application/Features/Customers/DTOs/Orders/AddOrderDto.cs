using CustomerOrder.Domain.ValueObjects;
using FluentValidation;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record AddOrderDto
{
    public AddOrderDto(Money totalPrice, List<AddOrderItemDto> items)
    {
        TotalPrice = totalPrice;
        Items = items;
    }

    public Money TotalPrice { get; set; }
    public List<AddOrderItemDto> Items { get; set; }
}

public class AddOrderValidator : AbstractValidator<AddOrderDto>
{
    public AddOrderValidator()
    {
        RuleFor(x => x.TotalPrice)
            .NotNull()
            .WithMessage("Total price is required.");

        RuleFor(x => x.TotalPrice.Value)
            .GreaterThan(0)
            .WithMessage("Total price must be greater than zero.")
            .When(x => x.TotalPrice != null!);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Items are required.");

        RuleFor(x => x.Items.Count)
            .GreaterThan(0)
            .WithMessage("Items count must be greater than zero.")
            .When(x => x.Items != null!);

        RuleForEach(x => x.Items)
            .SetValidator(new AddOrderItemValidator())
            .WithMessage("Invalid item in the order.");
    }
}
