using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Infrastructure.Shared.Services.Repository;
using Infrastructure.Shared.Specifications.DataSource;
using Domain.Todos.Services;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo>, ITodoRepository
{
    public TodoRepository(DataContext context) : base(context)
    {
    }

    protected override IDataSourceSpecification<Todo> DataSource
        => new TodoDataSourceSpecification(_context);

    protected override IDataModel<Todo> ToDataModel(Todo origin)
        => new TodoDataModel(origin);

    protected override void Transfer(Todo origin, IDataModel<Todo> dataModel)
        => ((TodoDataModel)dataModel).Transfer(origin);

    protected override async Task AddEntityAsync(IDataModel<Todo> entity)
        => await _context.Todo.AddAsync((TodoDataModel)entity);

    protected override void UpdateEntity(IDataModel<Todo> entity)
        => _context.Todo.Update((TodoDataModel)entity);

    protected override void RemoveEntity(IDataModel<Todo> entity)
        => _context.Todo.Remove((TodoDataModel)entity);

    public async Task<List<Todo>> FetchWithState(TodoState state, Guid? userId)
    {
        var query = _context.Todo
            .Where(x => x.OwnerUser!.Id == userId && x.State == state.Value);
        var result = await query.ToListAsync();

        return result
            .Select(x => DataSource.ToDomain(x))
            .ToList();
    }
}