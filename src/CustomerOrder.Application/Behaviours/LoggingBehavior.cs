using CustomerOrder.Common.Response;
using MediatR;
using Serilog;
using Serilog.Context;

namespace CustomerOrder.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.Information("Processing {@RequestName} with Request: {@Request}",
            requestName,
            request);

        var response = await next();

        if (response.IsFailure)
        {
            using (LogContext.PushProperty("Error", response.Error, true))
            {
                _logger.Error("Completed {@RequestName} with {@Error}",
                    requestName,
                    response.Error);
            }
        }

        _logger.Information("Completed {@RequestName} with Response: {@Response}",
            requestName,
            response);

        return response;
    }
}
