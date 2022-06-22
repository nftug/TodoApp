using Domain.Comment;
using Domain.Shared;

namespace Domain.Todo;

public class TodoModel : ModelBase
{
    public TodoTitle Title { get; private set; } = null!;
    public TodoDescription Description { get; private set; } = null!;
    public TodoPeriod Period { get; private set; } = null!;
    public TodoState State { get; private set; } = null!;
    public ICollection<CommentModel> Comments { get; private set; } = new List<CommentModel>();

    public static TodoModel CreateNew(
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
