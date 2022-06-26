using Domain.Todo;
using Infrastructure.Services.Repository;
using Domain.Interfaces;
using AutoMapper;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Todo;

public class TodoRepository : RepositoryBase<TodoModel>, ITodoRepository
{
    public TodoRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override async Task<List<TodoModel>> GetListAsync
        (IQueryable<IEntity<TodoModel>> query)
    {
        var _query = query.Cast<TodoDataModel>().Include(x => x.Comments);
        return await base.GetListAsync(_query);
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

    public async Task<List<TodoModel>> FetchWithState(TodoState state, Guid? userId)
    {
        var query = _context.Todo
            .Where(x => x.OwnerUser!.Id == userId && x.State == state.Value);
        return await GetListAsync(query);
    }
}