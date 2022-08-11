using Application.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoStateCommand : ICommand<Todo>
{
    public Guid? Id { get; set; }
    [TodoState]
    public string? State { get; set; }
}
