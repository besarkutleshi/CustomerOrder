using CustomerOrder.Application.Features.Customers.DTOs;
using CustomerOrder.Application.Features.Customers.Queries;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Common.Response;
using CustomerOrder.Domain.Aggregates;
using CustomerOrder.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CustomerOrder.UnitTests.Application.Features.Customers.Queries;

[TestFixture]
public class GetCustomersQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Customer, CustomerId>> _customerRepoMock;
    private GetCustomers.GetCustomersQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepoMock = new Mock<IGenericRepository<Customer, CustomerId>>();

        _unitOfWorkMock.Setup(u => u.Repository<Customer, CustomerId>())
                       .Returns(_customerRepoMock.Object);

        _handler = new GetCustomers.GetCustomersQueryHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCustomersExist()
    {
        var emptyResult = new PaginatedResult<Customer>(
            data: new List<Customer>(),
            totalRecords: 0,
            pageNumber: 1,
            pageSize: 10
        );

        _customerRepoMock
            .Setup(r => r.GetAllPaginatedAsync(It.IsAny<CancellationToken>(), 1, 10))
            .ReturnsAsync(emptyResult);

        var query = new GetCustomers.GetCustomersQuery(1, 10);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Response.Result.Should().BeNull();
    }

    [Test]
    public async Task Handle_ShouldReturnCustomerList_WhenCustomersExist()
    {
        var customer1 = new Customer("John", "Doe", new Address("Street A", "City", "State", "123"));
        var customer2 = new Customer("Jane", "Smith", new Address("Street B", "City", "State", "456"));

        var resultData = new PaginatedResult<Customer>(
            data: new List<Customer> { customer1, customer2 },
            totalRecords: 2,
            pageNumber: 1,
            pageSize: 10
        );

        _customerRepoMock
            .Setup(r => r.GetAllPaginatedAsync(It.IsAny<CancellationToken>(), 1, 10))
            .ReturnsAsync(resultData);

        var query = new GetCustomers.GetCustomersQuery(1, 10);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var customers = result.Response.Result.Should().BeOfType<List<CustomerDto>>().Subject;
        customers.Should().HaveCount(2);
        customers[0].FirstName.Should().Be("John");
        customers[1].FirstName.Should().Be("Jane");
    }
}
