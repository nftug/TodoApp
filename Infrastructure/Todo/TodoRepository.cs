using Domain.Todo;
using Infrastructure.Shared.Repository;
using Domain.Interfaces;

namespace Infrastructure.Todo;

public class TodoRepository : RepositoryBase<TodoModel>
{
    public TodoRepository
        (DataContext context, IDataSource<TodoModel> source)
        : base(context, source)
    {
    }
}