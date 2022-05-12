using Application.Core.Query;

namespace Application.Comments.Query
{
    public class QueryParameter : QueryParameterBase
    {
        public string? q { get; set; }
        public string? Content { get; set; }
    }
}