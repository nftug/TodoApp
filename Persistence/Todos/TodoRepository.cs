using Domain.Todos;
using Domain.Comments;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.DataModels;

namespace Persistence.Todos;

public class TodoRepository : ITodoRepository
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
                                               .Include(x => x.Comments)
                                               .FirstOrDefaultAsync(
                                                   x => x.Id == todo.Id
                                               );
        if (foundTodoDataModel == null)
            throw new NotFoundException();

        var todoDataModel = Transfer(todo, foundTodoDataModel);

        _context.Todos.Update(todoDataModel);
        await _context.SaveChangesAsync();

        return ToModel(todoDataModel);
    }

    private TodoDataModel ToDataModel(Todo todo)
    {
        return new TodoDataModel
        {
            Title = todo.Title.Value,
            Description = todo.Description?.Value,
            BeginDateTime = todo.BeginDateTime,
            DueDateTime = todo.DueDateTime,
            State = todo.State.Value,
            // Comments = todo.Comments,
            // TODO: Commentドメインを実装次第下の行を入れ替えること
            Comments = new List<CommentDataModel>(),
            CreatedDateTime = todo.CreatedDateTime,
            UpdatedDateTime = todo.UpdatedDateTime,
            OwnerUserId = todo.OwnerUserId
        };
    }

    private TodoDataModel Transfer(Todo todo, TodoDataModel todoDataModel)
    {
        todoDataModel.Title = todo.Title.Value;
        todoDataModel.Description = todo.Description?.Value;
        todoDataModel.BeginDateTime = todo.BeginDateTime;
        todoDataModel.DueDateTime = todo.DueDateTime;
        todoDataModel.State = todo.State.Value;
        todoDataModel.CreatedDateTime = todo.CreatedDateTime;
        todoDataModel.UpdatedDateTime = todo.UpdatedDateTime;
        todoDataModel.OwnerUserId = todo.OwnerUserId;

        return todoDataModel;
    }

    public async Task<Todo?> FindAsync(Guid id)
    {
        var todoDataModel = await _context.Todos
                                          .Include(x => x.Comments)
                                          .FirstOrDefaultAsync(
                                              x => x.Id == id
                                          );
        return todoDataModel != null ? ToModel(todoDataModel) : null;
    }

    private Todo ToModel(TodoDataModel todoDataModel)
    {
        return Todo.CreateFromRepository(
            id: todoDataModel.Id,
            title: new TodoTitle(todoDataModel.Title),
            description: !string.IsNullOrWhiteSpace(todoDataModel.Description)
                ? new TodoDescription(todoDataModel.Description) : null,
            beginDateTime: todoDataModel.BeginDateTime,
            dueDateTime: todoDataModel.DueDateTime,
            state: new TodoState(todoDataModel.State),
            // comments: todoDataModel.Comments,
            // TODO: Commentドメインを実装次第下の行を入れ替えること
            comments: new List<Comment>(),
            createdDateTime: todoDataModel.CreatedDateTime,
            updatedDateTime: todoDataModel.UpdatedDateTime,
            ownerUserId: todoDataModel?.OwnerUserId
        );
    }

    public async Task RemoveAsync(Guid id)
    {
        var todoDataModel = await _context.Todos.FindAsync(id);

        if (todoDataModel == null)
            throw new NotFoundException();

        _context.Todos.Remove(todoDataModel);
        await _context.SaveChangesAsync();
    }
}
