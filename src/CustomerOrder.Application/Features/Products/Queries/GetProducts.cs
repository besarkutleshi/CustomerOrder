using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Products.Queries;
public class GetProducts
{
    public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result>;

    public class GetProductQueryHandler : IRequestHandler<GetProductsQuery, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product, ProductId>();
            var result = await repo.GetAllPaginatedAsync(cancellationToken, request.PageNumber, request.PageSize);

            var dtos = result.Data.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.IsActive)).ToList();

            return Result.Success(Success.Ok(PaginatedResult<ProductDto>.Create(dtos, result.TotalRecords, result.PageNumber, result.PageSize)));
        }
    }
}
