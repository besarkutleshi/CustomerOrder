using CustomerOrder.Domain.ValueObjects;
using FluentValidation;
using static CustomerOrder.Application.Features.Products.Commands.AddProduct;

namespace CustomerOrder.Application.Features.Products.DTOs;
public record AddProductDto
{
    public AddProductDto(string name, Money price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
}

public class AddProductDtoValidator : AbstractValidator<AddProductCommand>
{
    public AddProductDtoValidator()
    {
        RuleFor(x => x.AddProductDto.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.AddProductDto.Price)
            .NotNull()
            .WithMessage("Price is required.")
            .Must(price => price.Value > 0)
            .WithMessage("Price must be greater than zero.");
    }
}