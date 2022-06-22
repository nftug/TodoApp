using Domain.Shared;

namespace Domain.Comment;

public class CommentModel : ModelBase
{
    public CommentContent Content { get; private set; } = null!;
    public Guid TodoId { get; private set; }

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
            CreatedDateTime = operationDateTime,
            UpdatedDateTime = operationDateTime,
            OwnerUserId = ownerUserId
        };
    }

    public void Edit(CommentContent content)
    {
        Content = content;
        UpdatedDateTime = DateTime.Now;
    }
}
