using CustomerOrder.Application.Behaviours;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace CustomerOrder.UnitTests.Application.Behaviours;
[TestFixture]
public class ValidationBehaviourTests
{
    public record TestRequest(string Name) : IRequest<string>;

    private ValidationBehaviour<TestRequest, string> _behaviour = null!;
    private RequestHandlerDelegate<string> _next;

    [SetUp]
    public void Setup()
    {
        _next = () => Task.FromResult("Success");
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenNoValidatorsExist()
    {
        _behaviour = new ValidationBehaviour<TestRequest, string>(Enumerable.Empty<IValidator<TestRequest>>());

        var request = new TestRequest("Valid");

        var result = await _behaviour.Handle(request, _next, CancellationToken.None);

        result.Should().Be("Success");
    }

    [Test]
    public async Task Handle_ShouldContinue_WhenValidationPasses()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock.Setup(v =>
                v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _behaviour = new ValidationBehaviour<TestRequest, string>(new[] { validatorMock.Object });

        var request = new TestRequest("Valid");

        var result = await _behaviour.Handle(request, _next, CancellationToken.None);

        result.Should().Be("Success");
    }

    [Test]
    public void Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock.Setup(v =>
                v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new("Name", "Name is required")
            }));

        _behaviour = new ValidationBehaviour<TestRequest, string>(new[] { validatorMock.Object });

        var request = new TestRequest("");

        var act = async () => await _behaviour.Handle(request, _next, CancellationToken.None);

        act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Name is required*");
    }
}
