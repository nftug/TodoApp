using Application.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoCommandDTO : ICommandDTO<Todo>
{
    public Guid? Id { get; init; }
    [TodoTitle]
    public string? Title { get; init; }
    [TodoDescription]
    public string? Description { get; init; }
    [TodoPeriod(ArgumentType.Start, "EndDate")]
    public DateTime? StartDate { get; init; }
    [TodoPeriod(ArgumentType.End, "StartDate")]
    public DateTime? EndDate { get; init; }
    [TodoState]
    public int? State { get; init; }
}
