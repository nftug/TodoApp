using Domain.Interfaces;

namespace Domain.Todo;

public class TodoQueryParameter : IQueryParameter<TodoModel>
{
    public string? Q { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Comment { get; init; }
    public string? UserName { get; init; }
    public int? State { get; init; }
    public Guid? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
