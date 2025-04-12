using CustomerOrder.Domain.ValueObjects;
using FluentValidation;

namespace CustomerOrder.Application.Features.Customers.DTOs.Orders;
public record AddOrderItemDto
{
    public AddOrderItemDto(ProductId productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public ProductId ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddOrderItemValidator : AbstractValidator<AddOrderItemDto>
{
    public AddOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.ProductId.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.")
            .When(x => x.ProductId != null!);

        RuleFor(x => x.Quantity)
            .NotNull()
            .WithMessage("Quantity is required.")
            .Must(x => x > 0)
            .WithMessage("Quantity must be greater than zero.");
    }
}