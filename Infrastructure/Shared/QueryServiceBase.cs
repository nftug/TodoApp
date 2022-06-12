namespace Infrastructure.Shared;

public abstract class QueryServiceBase<T, TQueryParameter>
{
    public DataContext _context { get; set; }

    public QueryServiceBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<T> GetFilteredQuery(TQueryParameter param);

    protected IEnumerable<(string, CombineMode, Guid)> Keywords(string? param)
    {
        if (string.IsNullOrWhiteSpace(param))
            yield break;

        string paramLower = param.Replace("\u3000", " ");
        string[] paramArray = paramLower.Split(' ');

        CombineMode mode = CombineMode.And;
        Guid blockId = Guid.NewGuid();

        for (int i = 0; i < paramArray.Length; i++)
        {
            CombineMode oldMode = mode;

            if (string.IsNullOrWhiteSpace(paramArray[i]) || paramArray[i] == "OR")
                continue;

            // 次の項目を読んで、現在がOR区分に当てはまるか判定
            if (i + 1 < paramArray.Length)
                mode = paramArray[i + 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // 前の項目を読んで、現在がOR区分に当てはまるか判定
            if (i - 1 > 0)
                mode = paramArray[i - 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // OR/ANDが変更されたら、ブロックを変える
            if (oldMode != mode)
                blockId = Guid.NewGuid();

            yield return (paramArray[i].ToLower(), mode, blockId);
        }
    }
}
