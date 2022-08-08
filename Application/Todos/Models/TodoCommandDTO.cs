using Application.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoCommandDTO : ICommandDTO<Todo>
{
    public Guid? Id { get; set; }
    [TodoTitle]
    public string? Title { get; set; }
    [TodoDescription]
    public string? Description { get; set; }
    [TodoPeriod(ArgumentType.Start, "EndDate")]
    public DateTime? StartDate { get; set; }
    [TodoPeriod(ArgumentType.End, "StartDate")]
    public DateTime? EndDate { get; set; }
    [TodoState]
    public int? State { get; set; }

    public TodoCommandDTO() { }

    public TodoCommandDTO(TodoResultDTO origin)
    {
        Id = origin.Id;
        Title = origin.Title;
        Description = origin.Description;
        StartDate = origin.StartDate;
        EndDate = origin.EndDate;
        State = origin.State.Value;
    }
}
