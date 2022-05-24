namespace Infrastructure.Shared;

public abstract class QueryServiceBase<T, TQueryParameter>
{
    public DataContext _context { get; set; }

    public QueryServiceBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<T> GetFilteredQuery(TQueryParameter param);

    protected IEnumerable<string> Keywords(string? param)
    {
        // TODO: OR検索にも対応させる

        if (string.IsNullOrWhiteSpace(param))
            yield break;

        string paramLower = param.Replace("\u3000", " ");
        string[] paramArray = paramLower.Split(' ');

        foreach (var _param in paramArray)
        {
            if (!string.IsNullOrWhiteSpace(_param))
                yield return _param.ToLower();
        }
    }

}