using Application.Core.Query;

namespace Application.TodoItems.Query
{
    public class QueryParameter : QueryParameterBase
    {
        public string? q { get; set; }
        public string? Name { get; set; }
        public string? Comment { get; set; }
        public bool? IsComplete { get; set; }
    }
}