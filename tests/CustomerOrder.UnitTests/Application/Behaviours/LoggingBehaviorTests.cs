using CustomerOrder.Application.Behaviours;
using CustomerOrder.Common.Response;
using FluentAssertions;
using MediatR;
using Moq;
using Serilog;

namespace CustomerOrder.UnitTests.Application.Behaviours;

[TestFixture]
public class LoggingBehaviorTests
{
    private Mock<ILogger> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
    }

    public record TestRequest(string Message) : IRequest<Result>;

    [Test]
    public async Task Handle_ShouldLogInformation_WhenRequestSucceeds()
    {
        var request = new TestRequest("Hello");

        var behavior = new LoggingBehavior<TestRequest, Result>(_loggerMock.Object);

        var expectedResult = Result.Success(Success.Ok());

        RequestHandlerDelegate<Result> next = () => Task.FromResult(expectedResult);

        var result = await behavior.Handle(request, next, CancellationToken.None);

        result.Should().Be(expectedResult);

        _loggerMock.Verify(log =>
            log.Information("Processing {@RequestName} with Request: {@Request}",
                nameof(TestRequest), request), Times.Once);

        _loggerMock.Verify(log =>
            log.Information("Completed {@RequestName} with Response: {@Response}",
                nameof(TestRequest), expectedResult), Times.Once);

        _loggerMock.Verify(log =>
            log.Error(It.IsAny<string>(), It.IsAny<object[]>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_ShouldLogError_WhenRequestFails()
    {
        var request = new TestRequest("Bad Request");

        var behavior = new LoggingBehavior<TestRequest, Result>(_loggerMock.Object);

        var errorResult = Result.Failure(Error.Validation("ValidationError", ["Invalid input"]));

        RequestHandlerDelegate<Result> next = () => Task.FromResult(errorResult);

        var result = await behavior.Handle(request, next, CancellationToken.None);

        result.Should().Be(errorResult);

        _loggerMock.Verify(log =>
            log.Information("Processing {@RequestName} with Request: {@Request}",
                nameof(TestRequest), request), Times.Once);

        _loggerMock.Verify(log =>
            log.Error("Completed {@RequestName} with {@Error}",
                nameof(TestRequest), errorResult.Error), Times.Once);

        _loggerMock.Verify(log =>
            log.Information("Completed {@RequestName} with Response: {@Response}",
                nameof(TestRequest), errorResult), Times.Once);
    }
}

