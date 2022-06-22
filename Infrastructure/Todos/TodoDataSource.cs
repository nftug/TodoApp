using AutoMapper;
using Domain.Interfaces;
using Domain.Todos;
using Infrastructure.DataModels;
using Infrastructure.Shared.DataSource;

namespace Infrastructure.Todos;

public class TodoDataSource : DataSourceBase<Todo>
{
    public TodoDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<IEntity<Todo>> Source => _context.Todos;

    public override Todo MapToDomain(IEntity<Todo> entity)
        => _mapper.Map<Todo>(entity);

    public override IEntity<Todo> MapToEntity(Todo item)
        => _mapper.Map<TodoDataModel>(item);

    public override void Transfer(Todo item, IEntity<Todo> entity)
        => _mapper.Map(item, entity);

    public override async Task AddEntityAsync(IEntity<Todo> entity)
        => await _context.Todos.AddAsync((TodoDataModel)entity);

    public override void UpdateEntity(IEntity<Todo> entity)
        => _context.Todos.Update((TodoDataModel)entity);

    public override void RemoveEntity(IEntity<Todo> entity)
        => _context.Todos.Remove((TodoDataModel)entity);
}
