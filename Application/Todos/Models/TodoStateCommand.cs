using Application.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoStateCommand : ICommandDTO<Todo>
{
    public Guid? Id { get; set; }
    [TodoState]
    public int? State { get; set; }
}
