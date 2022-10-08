using Domain.Shared.DTOs;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.DTOs;

public class TodoStateCommand : ICommand<Todo>
{
    public Guid? Id { get; set; }
    [TodoState]
    public string? State { get; set; }
}
