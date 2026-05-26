namespace PRN232.LMS.Services.Models.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Request processed successfully") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, object? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public PaginationMeta Pagination { get; set; } = new();
}
