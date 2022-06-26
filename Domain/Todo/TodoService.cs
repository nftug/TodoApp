using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Todo;

public class TodoService : DomainServiceBase<TodoModel>
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(
        IRepository<TodoModel> todoRepository
    )
    {
        _todoRepository = (ITodoRepository)todoRepository;
    }

    public async Task<List<TodoModel>> QueryWithState(TodoState state, Guid? userId)
    {
        return await _todoRepository.FetchWithState(state, userId);
    }
}
