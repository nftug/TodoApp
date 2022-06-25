using Domain.Todo;
using Infrastructure.Services.Repository;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.DataModels;

namespace Infrastructure.Todo;

public class TodoRepository : RepositoryBase<TodoModel>
{
    public TodoRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    protected override IQueryable<IEntity<TodoModel>> Source => _context.Todo;

    protected override IEntity<TodoModel> MapToEntity(TodoModel item)
        => _mapper.Map<TodoDataModel>(item);

    protected override async Task AddEntityAsync(IEntity<TodoModel> entity)
        => await _context.Todo.AddAsync((TodoDataModel)entity);

    protected override void UpdateEntity(IEntity<TodoModel> entity)
        => _context.Todo.Update((TodoDataModel)entity);

    protected override void RemoveEntity(IEntity<TodoModel> entity)
        => _context.Todo.Remove((TodoDataModel)entity);
}