namespace Application.Core.Pagination;

// TODO: あとで消去

public abstract class QueryParameterBase
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
}
