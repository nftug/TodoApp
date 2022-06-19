using Application.Shared.Interfaces;
using Domain.Todos;

namespace Application.Todos;

public class TodoCommandDTO : ICommandDTO<Todo>
{
    public Guid Id { get; init; }
    [TodoTitleAttribute]
    public string? Title { get; init; }
    [TodoDescriptionAttribute]
    public string? Description { get; init; }
    [TodoPeriodAttribute(Period.Begin, "DueDateTime")]
    public DateTime? BeginDateTime { get; init; }
    [TodoPeriodAttribute(Period.Due, "BeginDateTime")]
    public DateTime? DueDateTime { get; init; }
    [TodoStateAttribute]
    public int? State { get; init; }
}
