using Application.Core.Pagination;

namespace Application.TodoItems.Query
{
    public class QueryParameter : QueryParameterBase
    {
        public string? q { get; set; }
        public string? Name { get; set; }
        public string? Comment { get; set; }
        public string? UserName { get; set; }
        public bool? IsComplete { get; set; }
        public string? User { get; set; }
    }
}