using System.Net;
using System.Text.Json.Serialization;

namespace CustomerOrder.Common.Response;
public record Success
{
    public Success(object? response, SuccessType type, string message = null!)
    {
        Result = response;
        Type = type;
        Message = message;
    }

    public Success(SuccessType type)
    {
        Type = type;
    }

    public object? Result { get; }
    [JsonIgnore]
    public SuccessType Type { get; }
    public string Message { get; set; } = null!;

    public static Success Ok(object? response, string message = null!) => new(response, SuccessType.Success, message);
    public static Success Ok() => new(SuccessType.Success);
    public static Success Accept(string message) => new(message, SuccessType.Accepted);
    public static Success Created(object? response, string message = null!) => new(response, SuccessType.Created, message);
    public static Success NoContent() => new(SuccessType.NoContent);

    public HttpStatusCode GetHttpStatusCodeBySuccessType()
    {
        return Type switch
        {
            SuccessType.Success => HttpStatusCode.OK,
            SuccessType.Created => HttpStatusCode.Created,
            SuccessType.NoContent => HttpStatusCode.NoContent,
            SuccessType.Accepted => HttpStatusCode.Accepted,
            _ => HttpStatusCode.OK,
        };
    }
}