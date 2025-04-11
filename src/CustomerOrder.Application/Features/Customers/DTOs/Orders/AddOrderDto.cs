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
            .WithMessage("Total price is required.")
            .Must(x => x.Value > 0)
            .WithMessage("Total price must be greater than zero.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Items are required.")
            .Must(x => x.Count > 0)
            .WithMessage("At least one item is required.");

        RuleForEach(x => x.Items)
            .SetValidator(new AddOrderItemValidator())
            .WithMessage("Invalid item in the order.");
    }
}
