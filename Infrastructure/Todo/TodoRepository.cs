using Domain.Todo;
using Infrastructure.Services.Repository;
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