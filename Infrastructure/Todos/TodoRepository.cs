using Domain.Todos;
using Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared.Repository;
using Domain.Shared;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo, TodoDataModel>
{
    public TodoRepository(DataContext context) : base(context)
    {
    }

    public override async Task<Todo?> FindAsync(Guid id)
    {
        var data = await _context.Todos
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data != null ? ToModel(data) : null;
    }

    protected override TodoDataModel ToDataModel(Todo item)
    {
        return new()
        {
            Title = item.Title.Value,
            Description = item.Description?.Value,
            BeginDateTime = item.Period?.BeginDateTimeValue,
            DueDateTime = item.Period?.DueDateTimeValue,
            State = item.State.Value,
            Comments = new List<CommentDataModel>(),
            CreatedDateTime = item.CreatedDateTime,
            UpdatedDateTime = item.UpdatedDateTime,
            OwnerUserId = item.OwnerUserId
        };
    }

    protected override TodoDataModel Transfer
        (Todo item, TodoDataModel data)
    {
        data.Title = item.Title.Value;
        data.Description = item.Description?.Value;
        data.BeginDateTime = item.Period?.BeginDateTimeValue;
        data.DueDateTime = item.Period?.DueDateTimeValue;
        data.State = item.State.Value;
        data.CreatedDateTime = item.CreatedDateTime;
        data.UpdatedDateTime = item.UpdatedDateTime;
        data.OwnerUserId = item.OwnerUserId;

        return data;
    }

    protected override Todo ToModel(TodoDataModel data)
    {
        var comments = data.Comments
            .Select(x =>
                new Comment(
                    x.Id,
                    new(x.Content),
                    x.TodoId,
                    x.CreatedDateTime,
                    x.UpdatedDateTime,
                    x.OwnerUserId
                )
            )
            .ToList();

        return new(
            id: data.Id,
            title: new(data.Title),
            description: !string.IsNullOrWhiteSpace(data.Description)
                ? new(data.Description) : null,
            period: new(data.BeginDateTime, data.DueDateTime),
            state: new(data.State),
            comments: comments,
            createdDateTime: data.CreatedDateTime,
            updatedDateTime: data.UpdatedDateTime,
            ownerUserId: data?.OwnerUserId
        );
    }
}
