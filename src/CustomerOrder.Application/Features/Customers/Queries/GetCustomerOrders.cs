using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Application.Features.Products.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Queries;
public class GetCustomerOrders
{
    public record GetCustomerOrdersQuery(CustomerId CustomerId) : IRequest<Result>;

    public class GetCustomerOrdersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustomerOrdersQuery, Result>
    {
        public async Task<Result> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.Repository<Customer, CustomerId>();

            var customer = await repo.GetByIdAsync(request.CustomerId, cancellationToken);
            if(customer == null)
                return Result.Failure(Error.NotFound("NotFound", [$"Customer with id: '{request.CustomerId.Id}' not found."]));

            List<OrderDto> orders = [];
            if (customer.Orders?.Count > 0)
            {
                var products = await GetOrdersProductsAsync([.. customer.Orders], cancellationToken);

                orders = [.. customer.Orders.Select(item => new OrderDto(
                    item.OrderDate,
                    item.Status,
                    item.TotalPrice,
                    item.OrderItems.Select(ot => new OrderItemDto(
                        GetProduct(ot.ProductId, products),
                        ot.Quantity
                    )).ToList()
                )).OrderByDescending(x => x.OrderDate)];
            }

            return Result.Success(Success.Ok(orders));
        }

        private async Task<Dictionary<ProductId, Product>> GetOrdersProductsAsync(List<Order> orders, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.Repository<Product, ProductId>();

            List<ProductId> productIds = orders
                .SelectMany(order => order.OrderItems)
                .Select(item => item.ProductId)
                .Distinct()
                .ToList();

            var products = await repo.GetAllAsyncDict(cancellationToken, x => productIds.Contains(x.Id));

            return products;
        }

        private static ProductDto GetProduct(ProductId productId, Dictionary<ProductId, Product> products)
        {
            if (products.TryGetValue(productId, out var product))
            {
                return new ProductDto(productId, product.Name, product.Price);
            }

            throw new ArgumentException($"Product with id: '{productId.Id}' not found", nameof(productId));
        }
    }
}
