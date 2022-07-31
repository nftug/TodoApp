using Domain.Comments.Entities;
using Domain.Shared.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Entities;

public class Todo : ModelBase
{
    public TodoTitle Title { get; private set; } = null!;
    public TodoDescription Description { get; private set; } = null!;
    public TodoPeriod Period { get; private set; } = null!;
    public TodoState State { get; private set; } = null!;
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public Todo(
        Guid id,
        DateTime createdOn,
        DateTime updatedOn,
        Guid? ownerUserId,
        TodoTitle title,
        TodoDescription description,
        TodoPeriod period,
        TodoState state,
        ICollection<Comment> comments
    )
        : base(id, createdOn, updatedOn, ownerUserId)
    {
        Title = title;
        Description = description;
        Period = period;
        State = state;
        Comments = comments;
    }

    private Todo() { }

    public static Todo CreateNew(
        TodoTitle title,
        TodoDescription description,
        TodoPeriod period,
        TodoState state,
        Guid ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new()
        {
            Title = title,
            Description = description,
            Period = period,
            State = state,
            CreatedOn = operationDateTime,
            UpdatedOn = operationDateTime,
            OwnerUserId = ownerUserId
        };
    }

    public void Edit(
        TodoTitle title,
        TodoDescription description,
        TodoPeriod period,
        TodoState state
    )
    {
        Title = title;
        Description = description;
        Period = period;
        State = state;
        UpdatedOn = DateTime.Now;
    }
}
