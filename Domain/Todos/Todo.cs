using Domain.Comments;
using Domain.Shared;

namespace Domain.Todos;

public class Todo : ModelBase
{
    public TodoTitle Title { get; private set; } = null!;
    public TodoDescription Description { get; private set; } = null!;
    public TodoPeriod Period { get; private set; } = null!;
    public TodoState State { get; private set; } = null!;
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public static Todo CreateNew(
        TodoTitle title,
        TodoDescription description,
        TodoPeriod period,
        TodoState state,
        string ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new()
        {
            Title = title,
            Description = description,
            Period = period,
            State = state,
            CreatedDateTime = operationDateTime,
            UpdatedDateTime = operationDateTime,
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
        UpdatedDateTime = DateTime.Now;
    }
}
