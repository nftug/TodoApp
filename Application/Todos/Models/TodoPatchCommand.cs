using Application.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoPatchCommand : ICommand<Todo>
{
    public Guid? Id { get; set; }
    [TodoTitle(isPatch: true)]
    public string? Title { get; set; }
    [TodoDescription(isPatch: true)]
    public string? Description { get; set; }
    [TodoPeriod(ArgumentType.Start, "EndDate", isPatch: true)]
    public DateTime? StartDate { get; set; }
    [TodoPeriod(ArgumentType.End, "StartDate", isPatch: true)]
    public DateTime? EndDate { get; set; }
    [TodoState(isPatch: true)]
    public string? State { get; set; }

    public TodoPatchCommand() { }

    public TodoPatchCommand(TodoResultDTO origin)
    {
        Id = origin.Id;
        Title = origin.Title;
        Description = origin.Description;
        StartDate = origin.StartDate;
        EndDate = origin.EndDate;
        State = origin.State;
    }
}
