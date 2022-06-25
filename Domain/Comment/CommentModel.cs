using Domain.Shared;
using Domain.Todo;

namespace Domain.Comment;

public class CommentModel : ModelBase
{
    public CommentContent Content { get; private set; } = null!;
    public Guid TodoId { get; private set; }
    public TodoModel Todo { get; set; } = null!;

    public static CommentModel CreateNew(
        CommentContent content,
        Guid todoId,
        Guid ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new()
        {
            Content = content,
            TodoId = todoId,
            CreatedOn = operationDateTime,
            UpdatedOn = operationDateTime,
            OwnerUserId = ownerUserId
        };
    }

    public void Edit(CommentContent content)
    {
        Content = content;
        UpdatedOn = DateTime.Now;
    }
}
