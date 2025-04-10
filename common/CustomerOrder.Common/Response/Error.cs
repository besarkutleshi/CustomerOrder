using System.Net;

namespace CustomerOrder.Common.Response;

public record Error
{
    public Error(string code, List<string> errorMessages, ErrorType type, object response = null!)
    {
        Code = code;
        ErrorMessages = errorMessages;
        Type = type;
        Response = response;
    }

    public string Code { get; }
    public List<string> ErrorMessages { get; }
    public ErrorType Type { get; set; }
    public object Response { get; set; } = null!;

    public static Error NotFound(string code, List<string> errorMessages)
        => new(code, errorMessages, ErrorType.NotFound);

    public static Error Validation(string code, List<string> errorMessages, object response = null!)
        => new(code, errorMessages, ErrorType.Validation, response);

    public static Error Conflict(string code, List<string> errorMessages, object response = null!)
        => new(code, errorMessages, ErrorType.Conflict, response);

    public static Error Failure(string code, List<string> errorMessages)
        => new(code, errorMessages, ErrorType.Failure);

    public HttpStatusCode GetHttpStatusCodeByErrorType()
    {
        return Type switch
        {
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Failure => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
