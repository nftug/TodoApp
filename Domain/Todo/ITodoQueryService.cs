using Domain.Interfaces;

namespace Domain.Todo;

public interface ITodoQueryService : IQueryService<TodoModel>
{
    IQueryable<IEntity<TodoModel>> QueryWithState(TodoState state, Guid? userId);
}
