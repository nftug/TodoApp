using Domain.Shared.DTOs;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.DTOs;

public class TodoCommand : ICommand<Todo>
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
    public string? State { get; set; }

    public TodoCommand() { }

    public TodoCommand(TodoResultDTO origin)
    {
        Id = origin.Id;
        Title = origin.Title;
        Description = origin.Description;
        StartDate = origin.StartDate;
        EndDate = origin.EndDate;
        State = origin.State;
    }
}
