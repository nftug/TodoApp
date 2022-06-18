namespace Infrastructure.Shared;

public abstract class QueryServiceBase<T, TQueryParameter>
{
    protected readonly DataContext _context;

    public QueryServiceBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<T> GetFilteredQuery(TQueryParameter param);

    protected static IEnumerable<Keyword> GetKeyword(string? param)
        => Keyword.CreateFromRawString(param);
}
