using Domain.Shared.Interfaces;
using Domain.Shared.Services;
using Domain.Todos.Entities;
using Domain.Todos.Interfaces;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Services;

public class TodoService : DomainServiceBase<Todo>
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(IRepository<Todo> todoRepository)
        : base(todoRepository)
    {
        _todoRepository = (ITodoRepository)todoRepository;
    }

    public async Task<List<Todo>> QueryWithState(TodoState state, Guid? userId)
        => await _todoRepository.FetchWithState(state, userId);
}
