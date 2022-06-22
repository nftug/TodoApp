using Domain.Todos;
using Infrastructure.Shared.Repository;
using Domain.Interfaces;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo>
{
    public TodoRepository
        (DataContext context, IDataSource<Todo> source)
        : base(context, source)
    {
    }
}