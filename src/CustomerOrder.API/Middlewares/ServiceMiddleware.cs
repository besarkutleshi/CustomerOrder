using CustomerOrder.Common.Helpers;
using CustomerOrder.Common.Response;
using CustomerOrder.Infrastructure.Extensions;
using FluentValidation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomerOrder.API.Middlewares;

public class ServiceMiddleware(Serilog.ILogger logger, IRequestService requestService) : IMiddleware
{
    private readonly IRequestService _requestService = requestService;

    public static JsonSerializerOptions JsonSerializerOptions { get; private set; } = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await ValidatePagination(context);

            await next(context);
        }
        catch (ValidationException ex)
        {
            var error = Result.Failure(Error.Failure("Validation.Exception", ex.Errors.Select(x => x.ErrorMessage).ToList()));

            await context.WriteBody(error.Error.GetHttpStatusCodeByErrorType(), JsonSerializerOptions, error);
        }
        catch (Exception ex)
        {
            logger.Error("Unexpected ERROR: Message: {ex.Message}, Stack: {ex.StackTrace}", ex.Message, ex.StackTrace);

            var error = Result.Failure(Error.Failure("Application.Exception", ["Something went wrong, please try again later or contact administrator."]));

            await context.WriteBody(error.Error.GetHttpStatusCodeByErrorType(), JsonSerializerOptions, error);
        }
    }

    private static async Task ValidatePagination(HttpContext context)
    {
        var query = context.Request.Query;

        if (query.ContainsKey("pageNumber") || query.ContainsKey("pageSize"))
        {
            if (query.TryGetValue("pageNumber", out var pageNumberValues) &&
                (!int.TryParse(pageNumberValues, out int pageNumber) || pageNumber < 1))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid pageNumber. It must be a positive integer.");
                return;
            }

            if (query.TryGetValue("pageSize", out var pageSizeValues) &&
                (!int.TryParse(pageSizeValues, out int pageSize) || pageSize < 1 || pageSize > 100))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid pageSize. It must be between 1 and 100.");
                return;
            }
        }
    }
}
