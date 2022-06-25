using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Todo;

public class TodoService : DomainServiceBase<TodoModel>
{
    private readonly IRepository<TodoModel> _todoRepository;
    private readonly ITodoQueryService _queryService;

    public TodoService(
        IRepository<TodoModel> todoRepository,
        IQueryService<TodoModel> queryService
    )
    {
        _todoRepository = todoRepository;
        _queryService = (ITodoQueryService)queryService;
    }

    public IQueryable<IEntity<TodoModel>> QueryWithState(TodoState state, Guid? userId)
    {
        return _queryService.QueryWithState(state, userId);
    }
}
