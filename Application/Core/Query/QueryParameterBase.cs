namespace Application.Core.Query
{
    public abstract class QueryParameterBase
    {
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }
}