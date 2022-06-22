using Application.Shared.Interfaces;
using Domain.Todo;

namespace Application.Todo;

public class TodoCommandDTO : ICommandDTO<TodoModel>
{
    public Guid? Id { get; init; }
    [TodoTitle]
    public string? Title { get; init; }
    [TodoDescription]
    public string? Description { get; init; }
    [TodoPeriod(Period.Begin, "EndDate")]
    public DateTime? StartDate { get; init; }
    [TodoPeriod(Period.Due, "StartDate")]
    public DateTime? EndDate { get; init; }
    [TodoState]
    public int? State { get; init; }
}
