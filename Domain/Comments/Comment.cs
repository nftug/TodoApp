using Domain.Shared;

namespace Domain.Comments;

public class Comment : ModelBase
{
    public CommentContent Content { get; private set; } = null!;
    public Guid TodoId { get; private set; }

    public static Comment CreateNew(
        CommentContent content,
        Guid todoId,
        string ownerUserId
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
