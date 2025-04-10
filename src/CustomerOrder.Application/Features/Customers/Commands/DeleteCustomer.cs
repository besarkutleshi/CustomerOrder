using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Commands;
public class DeleteCustomer
{
    public record DeleteCustomerCommand(CustomerId CustomerId) : IRequest<Result>;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Customer, CustomerId>();

            var customer = await repo.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure(Error.NotFound("NotFound", [$"Customer with ID {request.CustomerId} not found."]));

            repo.Delete(customer);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.NoContent());
        }
    }
}
