namespace Infrastructure.Shared;

public abstract class PaginationQueryParameterBase
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
}
