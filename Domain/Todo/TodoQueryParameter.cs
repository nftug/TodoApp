using Domain.Interfaces;

namespace Domain.Todo;

public class TodoQueryParameter : IQueryParameter<TodoModel>
{
    public string? Q { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public string? UserName { get; set; }
    public int? State { get; set; }
    public Guid? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
