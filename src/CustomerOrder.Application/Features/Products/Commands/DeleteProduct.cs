using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Products.Commands;
public class DeleteProduct
{
    public record DeleteProductCommand(ProductId ProductId) : IRequest<Result>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductId is null || request.ProductId.Id <= 0)
                return Result.Failure(Error.Validation("Validation", ["Product Id should be greater than 0."]));

            var repo = _unitOfWork.Repository<Product, ProductId>();

            var product = await repo.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
                return Result.Failure(Error.NotFound("NotFound", [$"Product with id: '{request.ProductId.Id}' not found"]));

            product.Deactivate();

            repo.Update(product);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.NoContent());
        }
    }
}
