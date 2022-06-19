using Application.Shared.Interfaces;
using Domain.Todos;

namespace Application.Todos;

public class TodoCommandDTO : ICommandDTO<Todo>
{
    public Guid Id { get; init; }
    [TodoTitle]
    public string? Title { get; init; }
    [TodoDescription]
    public string? Description { get; init; }
    [TodoPeriod(Period.Begin, "DueDateTime")]
    public DateTime? BeginDateTime { get; init; }
    [TodoPeriod(Period.Due, "BeginDateTime")]
    public DateTime? DueDateTime { get; init; }
    [TodoState]
    public int? State { get; init; }
}
