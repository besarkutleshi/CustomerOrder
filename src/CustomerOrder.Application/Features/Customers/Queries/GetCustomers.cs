using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using MediatR;

namespace CustomerOrder.Application.Features.Customers.Queries;
public class GetCustomers
{
    public record GetCustomersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result>;

    public class GetCustomersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustomersQuery, Result>
    {
        public async Task<Result> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.Repository<Customer, CustomerId>();
            var customers = await repo.GetAllPaginatedAsync(cancellationToken, request.PageNumber, request.PageSize);

            List<CustomerDto> customerDtos = null!;
            if (customers.TotalRecords > 0)
            {
                customerDtos = [];
                foreach (var item in customers.Data)
                {
                    customerDtos.Add(new CustomerDto(item.Id, item.FirstName, item.LastName, item.Address));
                }
            }

            return Result.Success(Success.Ok(customerDtos));
        }
    }
}
