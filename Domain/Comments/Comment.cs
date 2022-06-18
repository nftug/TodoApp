using Domain.Shared;

namespace Domain.Comments;

public class Comment : ModelBase
{
    public CommentContent Content { get; private set; }
    public Guid TodoId { get; private set; }

    public Comment(
        Guid id,
        CommentContent content,
        Guid todoId,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        Id = id;
        Content = content;
        TodoId = todoId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        OwnerUserId = ownerUserId;
    }

    public static Comment CreateNew(
        CommentContent content,
        Guid todoId,
        string ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new Comment(
            id: new Guid(),  // Guidの生成はEF Coreに任せる
            content: content,
            todoId: todoId,
            createdDateTime: operationDateTime,
            updatedDateTime: operationDateTime,
            ownerUserId: ownerUserId
        );
    }

    public void Edit(CommentContent content)
    {
        Content = content;
        UpdatedDateTime = DateTime.Now;
    }

}
