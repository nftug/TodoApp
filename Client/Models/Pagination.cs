namespace Client.Models;

public class Pagination<T>
{
    public long TotalItems { get; init; }
    public int CurrentPage { get; init; }
    public int? NextPage { get; init; }
    public int? PreviousPage { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Results { get; init; } = null!;
}