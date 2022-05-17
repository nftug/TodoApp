namespace Domain.Comments;

public class Comment
{
    public Guid Id { get; set; }
    public CommentContent Content { get; private set; }
    public Guid TodoId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public string? OwnerUserId { get; private set; }

    private Comment(
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

    public static Comment CreateFromRepository(
        Guid id,
        CommentContent content,
        Guid todoId,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        return new Comment(
            id: id,  // Guidの生成はEF Coreに任せる
            content: content,
            todoId: todoId,
            createdDateTime: createdDateTime,
            updatedDateTime: updatedDateTime,
            ownerUserId: ownerUserId
        );
    }

    public void Edit(
        CommentContent content
    )
    {
        Content = content;
        UpdatedDateTime = DateTime.Now;
    }

}
