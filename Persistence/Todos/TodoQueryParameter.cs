using Persistence.Shared;

namespace Persistence.Todos;

public class TodoQueryParameter : PaginationQueryParameterBase
{
    public string? q { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public string? UserName { get; set; }
    public int? State { get; set; }
    public string? UserId { get; set; }
}
