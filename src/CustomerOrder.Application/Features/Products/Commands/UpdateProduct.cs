using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Products.Commands;
public class UpdateProduct
{
    public record UpdateProductCommand(UpdateProductDto UpdateProductDto) : IRequest<Result>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product, ProductId>();
            var product = await repo.GetByIdAsync(request.UpdateProductDto.Id, cancellationToken);
            if (product == null)
                return Result.Failure(Error.NotFound("NotFound", [$"Product with id: '{request.UpdateProductDto.Id.Id}' not found"]));

            product.Update(request.UpdateProductDto.Name, request.UpdateProductDto.Price);

            repo.Update(product);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.Ok());
        }
    }
}
