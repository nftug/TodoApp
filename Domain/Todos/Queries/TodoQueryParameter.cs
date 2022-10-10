using Domain.Shared.Queries;
using Domain.Todos.Entities;

namespace Domain.Todos.Queries;

public class TodoQueryParameter : IQueryParameter<Todo>
{
    public string? Q { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public string? UserName { get; set; }
    public string? State { get; set; }
    public Guid? UserId { get; set; }
    public int? StartIndex { get; set; } = 0;
    public int? Limit { get; set; } = 10;
    public string Sort { get; set; } = "-UpdatedOn";
}
