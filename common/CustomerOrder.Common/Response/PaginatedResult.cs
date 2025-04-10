namespace CustomerOrder.Common.Response;

public class PaginatedResult<T>(List<T> data, int totalRecords, int pageNumber, int pageSize)
{
    public List<T> Data { get; set; } = data;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalRecords { get; set; } = totalRecords;
    public int TotalPages { get; set; } = (int)Math.Ceiling((double)totalRecords / pageSize);

    public static PaginatedResult<T> Create(List<T> data, int totalRecords, int pageNumber, int pageSize)
        => new(data, totalRecords, pageNumber, pageSize);
}
