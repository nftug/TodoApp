namespace Domain.Todos;

public interface ITodoRepository
{
    Task<Todo> CreateAsync(Todo todo);
    Task<Todo> UpdateAsync(Todo todo);
    Task<Todo?> FindAsync(Guid id);
    Task RemoveAsync(Guid id);
}
