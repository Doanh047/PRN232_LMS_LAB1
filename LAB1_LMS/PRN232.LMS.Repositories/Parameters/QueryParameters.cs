namespace PRN232.LMS.Repositories.Parameters;

public class QueryParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;

    public int Size
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }

    public string? Fields { get; set; }
    public string? Expand { get; set; }
}
