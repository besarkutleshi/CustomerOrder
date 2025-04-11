using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Products.Commands;
public class AddProduct
{
    public record AddProductCommand(AddProductDto AddProductDto) : IRequest<Result>;

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product, ProductId>();
            var product = new Product(request.AddProductDto.Name, request.AddProductDto.Price);

            await repo.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.Created(request.AddProductDto));
        }
    }
}
