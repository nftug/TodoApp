using Infrastructure.Services.Repository;
using Domain.Shared.Interfaces;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Comments;
using Domain.Comments.Entities;
using Domain.Todos.Entities;
using Domain.Todos.Interfaces;
using Domain.Todos.ValueObjects;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo>, ITodoRepository
{
    public TodoRepository(
        DataContext context,
        IQueryService<Todo> queryService
    )
        : base(context, queryService)
    {
    }

    internal TodoRepository() { }

    private static CommentRepository CommentRepository => new();

    protected override IQueryable<IDataModel<Todo>> Source
        => _context.Todo
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .Select(x => new TodoDataModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                OwnerUserId = x.OwnerUserId,
                OwnerUser = x.OwnerUser != null
                    ? new UserDataModel<Guid> { UserName = x.OwnerUser.UserName }
                    : null,
                Title = x.Title,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                State = x.State,
                Comments = x.Comments
                    .OrderByDescending(x => x.CreatedOn)
                    .ToList()
            });

    protected override IDataModel<Todo> ToDataModel(Todo origin)
        => new TodoDataModel(origin);

    internal override Todo ToDomain(IDataModel<Todo> origin, bool recursive = true)
    {
        var _origin = (TodoDataModel)origin;

        return new Todo(
            id: _origin.Id,
            createdOn: _origin.CreatedOn,
            updatedOn: _origin.UpdatedOn,
            ownerUserId: _origin.OwnerUserId,
            title: new(_origin.Title),
            description: new(_origin.Description),
            period: new(_origin.StartDate, _origin.EndDate),
            state: new(_origin.State),
            comments: recursive
                ? _origin.Comments
                    .Select(x => CommentRepository.ToDomain(x, recursive: false))
                    .ToList()
                : new List<Comment>()
        );
    }

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
            .Select(x => ToDomain(x))
            .ToList();
    }
}