using CustomerOrder.Domain.ValueObjects;
using FluentValidation;
using static CustomerOrder.Application.Features.Products.Commands.UpdateProduct;

namespace CustomerOrder.Application.Features.Products.DTOs;
public record UpdateProductDto
{
    public UpdateProductDto(ProductId id, string name, Money price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public ProductId Id { get; set; }
    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
}

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.UpdateProductDto.Id)
            .NotNull()
            .WithMessage("Id is required.");

        RuleFor(x => x.UpdateProductDto.Id.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greaeter than 0.")
            .When(x => x.UpdateProductDto.Id != null!);

        RuleFor(x => x.UpdateProductDto.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.UpdateProductDto.Price)
            .NotNull()
            .WithMessage("Price is required.");

        RuleFor(x => x.UpdateProductDto.Price.Value)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.")
            .When(x => x.UpdateProductDto.Price != null!);
    }
}
