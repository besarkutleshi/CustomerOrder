using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Commands;
public class AddCustomer
{
    public record AddCustomerCommand(AddCustomerDto AddCustomerDto) : IRequest<Result>;
    
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Customer, CustomerId>();

            var customer = new Customer(request.AddCustomerDto.FirstName, request.AddCustomerDto.LastName, request.AddCustomerDto.Address);

            await repo.AddAsync(customer, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result.Success(Success.Created(request.AddCustomerDto));
        }
    }
}
