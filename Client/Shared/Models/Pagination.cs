namespace Client.Shared.Models;

public class Pagination<T>
{
    public long TotalItems { get; init; }
    public int CurrentPage { get; init; }
    public int? NextPage { get; init; }
    public int? PreviousPage { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Results { get => _results; init => _results = value; }

    private IEnumerable<T> _results = null!;

    public void SetResults(IEnumerable<T> value) => _results = value;
}