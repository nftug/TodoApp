using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Infrastructure.Todos;

public class TodoQueryParameter : IQueryParameter<TodoDataModel>
{
    public string? q { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public string? UserName { get; set; }
    public int? State { get; set; }
    public string? UserId { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}
