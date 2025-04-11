using CustomerOrder.Application.Features.Customers.DTOs.Orders;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Commands;
public class AddOrder
{
    public record AddOrderCommand(CustomerId CustomerId, AddOrderDto AddOrderDto) : IRequest<Result>;

    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result>
    {
        private IUnitOfWork _unitOfWork;

        public AddOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Customer, CustomerId>();

            var customer = await repo.GetByIdAsync(request.CustomerId, cancellationToken);
            if(customer is null)
                return Result.Failure(Error.NotFound("NotFound", [$"Customer with id {request.CustomerId} not found"]));

            var orderItems = request.AddOrderDto.Items
                .Select(x => new OrderItem(x.ProductId, x.Quantity))
                .ToList();

            var order = new Order(request.AddOrderDto.TotalPrice, orderItems);

            customer.AddOrder(order);

            repo.Update(customer);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.Created(request.AddOrderDto));
        }
    }
}
