using Domain.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Services;

public interface ITodoRepository : IRepository<Todo>
{
    Task<List<Todo>> FetchWithState(TodoState state, Guid? userId);
}
