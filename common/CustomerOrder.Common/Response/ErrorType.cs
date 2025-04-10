namespace CustomerOrder.Common.Response;

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    Conflict = 2,
    NotFound = 3,
}