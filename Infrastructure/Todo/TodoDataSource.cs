using AutoMapper;
using Domain.Interfaces;
using Domain.Todo;
using Infrastructure.DataModels;
using Infrastructure.Shared.DataSource;

namespace Infrastructure.Todo;

public class TodoDataSource : DataSourceBase<TodoModel>
{
    public TodoDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<IEntity<TodoModel>> Source => _context.Todo;

    public override IEntity<TodoModel> MapToEntity(TodoModel item)
        => _mapper.Map<TodoDataModel>(item);

    public override async Task AddEntityAsync(IEntity<TodoModel> entity)
        => await _context.Todo.AddAsync((TodoDataModel)entity);

    public override void UpdateEntity(IEntity<TodoModel> entity)
        => _context.Todo.Update((TodoDataModel)entity);

    public override void RemoveEntity(IEntity<TodoModel> entity)
        => _context.Todo.Remove((TodoDataModel)entity);
}
