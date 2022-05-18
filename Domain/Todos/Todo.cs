using Domain.Shared;
using Domain.Comments;

namespace Domain.Todos;

public class Todo
{
    public Guid Id { get; set; }
    public TodoTitle Title { get; private set; }
    public TodoDescription? Description { get; private set; }
    public TodoPeriod? Period { get; private set; }
    public TodoState State { get; private set; }
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public string? OwnerUserId { get; private set; }

    public Todo(
        Guid id,
        TodoTitle title,
        TodoDescription? description,
        TodoPeriod? period,
        TodoState state,
        ICollection<Comment> comments,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        Id = id;
        Title = title;
        Description = description;
        Period = period;
        State = state;
        Comments = comments;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        OwnerUserId = ownerUserId;
    }

    public static Todo CreateNew(
        TodoTitle title,
        TodoDescription? description,
        TodoPeriod? period,
        TodoState state,
        string ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new Todo(
            id: new Guid(),
            title: title,
            description: description,
            period: period,
            state: state,
            comments: new List<Comment>(),
            createdDateTime: operationDateTime,
            updatedDateTime: operationDateTime,
            ownerUserId: ownerUserId
        );
    }

    public void Edit(
        TodoTitle title,
        TodoDescription? description,
        TodoPeriod? period,
        TodoState state
    )
    {
        Title = title;
        Description = description;
        Period = period;
        State = state;
        UpdatedDateTime = DateTime.Now;
    }
}
