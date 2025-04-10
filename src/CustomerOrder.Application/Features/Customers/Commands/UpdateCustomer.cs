using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Commands;
public class UpdateCustomer
{
    public record UpdateCustomerRecord(UpdateCustomerDto UpdateCustomerDto) : IRequest<Result>;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerRecord, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCustomerRecord request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Customer, CustomerId>();

            var customer = await repo.GetByIdAsync(request.UpdateCustomerDto.Id, cancellationToken);
            if (customer == null)
                return Result.Failure(Error.NotFound("NotFound", [$"Customer with ID {request.UpdateCustomerDto.Id.Id} not found."]));

            customer.Update(request.UpdateCustomerDto.FirstName, request.UpdateCustomerDto.LastName, request.UpdateCustomerDto.Address);

            repo.Update(customer);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.NoContent());
        }
    }
}
