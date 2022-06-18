using Domain.Todos;
using Domain.Comments;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;

namespace Infrastructure.Todos;

public class TodoRepository : IRepository<Todo, TodoDataModel>
{
    private readonly DataContext _context;

    public TodoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Todo> CreateAsync(Todo todo)
    {
        var todoDataModel = ToDataModel(todo);
        await _context.Todos.AddAsync(todoDataModel);
        await _context.SaveChangesAsync();

        return ToModel(todoDataModel);
    }

    public async Task<Todo> UpdateAsync(Todo todo)
    {
        var foundTodoDataModel = await _context.Todos
            .FirstOrDefaultAsync(x => x.Id == todo.Id);

        if (foundTodoDataModel == null)
            throw new NotFoundException();

        var todoDataModel = Transfer(todo, foundTodoDataModel);

        _context.Todos.Update(todoDataModel);
        await _context.SaveChangesAsync();

        return ToModel(todoDataModel);
    }

    public async Task<Todo?> FindAsync(Guid id)
    {
        var todoDataModel = await _context.Todos
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);

        return todoDataModel != null
            ? ToModel(todoDataModel) : null;
    }

    public async Task<List<Todo>> GetPaginatedListAsync
        (IQueryable<TodoDataModel> query, IQueryParameter param)
    {
        var (page, limit) = (param.Page, param.Limit);

        return await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => ToModel(x))
            .ToListAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var todoDataModel = await _context.Todos.FindAsync(id);

        if (todoDataModel == null)
            throw new NotFoundException();

        _context.Todos.Remove(todoDataModel);
        await _context.SaveChangesAsync();
    }

    private static TodoDataModel ToDataModel(Todo todo)
    {
        return new()
        {
            Title = todo.Title.Value,
            Description = todo.Description?.Value,
            BeginDateTime = todo.Period?.BeginDateTimeValue,
            DueDateTime = todo.Period?.DueDateTimeValue,
            State = todo.State.Value,
            Comments = new List<CommentDataModel>(),
            CreatedDateTime = todo.CreatedDateTime,
            UpdatedDateTime = todo.UpdatedDateTime,
            OwnerUserId = todo.OwnerUserId
        };
    }

    private static TodoDataModel Transfer
        (Todo todo, TodoDataModel todoDataModel)
    {
        todoDataModel.Title = todo.Title.Value;
        todoDataModel.Description = todo.Description?.Value;
        todoDataModel.BeginDateTime = todo.Period?.BeginDateTimeValue;
        todoDataModel.DueDateTime = todo.Period?.DueDateTimeValue;
        todoDataModel.State = todo.State.Value;
        todoDataModel.CreatedDateTime = todo.CreatedDateTime;
        todoDataModel.UpdatedDateTime = todo.UpdatedDateTime;
        todoDataModel.OwnerUserId = todo.OwnerUserId;

        return todoDataModel;
    }

    private static Todo ToModel(TodoDataModel todoDataModel)
    {
        var comments = todoDataModel.Comments
            .Select(x => new Comment(
                x.Id,
                new(x.Content),
                x.TodoId,
                x.CreatedDateTime,
                x.UpdatedDateTime,
                x.OwnerUserId
            ))
            .ToList();

        return new(
            id: todoDataModel.Id,
            title: new(todoDataModel.Title),
            description: !string.IsNullOrWhiteSpace(todoDataModel.Description)
                ? new(todoDataModel.Description) : null,
            period: new(todoDataModel.BeginDateTime, todoDataModel.DueDateTime),
            state: new(todoDataModel.State),
            comments: comments,
            createdDateTime: todoDataModel.CreatedDateTime,
            updatedDateTime: todoDataModel.UpdatedDateTime,
            ownerUserId: todoDataModel?.OwnerUserId
        );
    }
}
