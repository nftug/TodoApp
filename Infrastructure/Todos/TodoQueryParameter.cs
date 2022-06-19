using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Infrastructure.Todos;

public class TodoQueryParameter : IQueryParameter<TodoDataModel>
{
    public string? Q { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Comment { get; init; }
    public string? UserName { get; init; }
    public int? State { get; init; }
    public string? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
