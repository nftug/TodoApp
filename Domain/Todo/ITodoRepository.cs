using Domain.Interfaces;

namespace Domain.Todo;

public interface ITodoRepository : IRepository<TodoModel>
{
    Task<List<TodoModel>> FetchWithState(TodoState state, Guid? userId);
}
