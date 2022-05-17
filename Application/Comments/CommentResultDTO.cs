using Domain.Comments;

namespace Application.Comments;

public class CommentResultDTO
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid TodoId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string? OwnerUserId { get; set; }

    public CommentResultDTO(
        Guid id,
        string content,
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

    public static CommentResultDTO CreateResultDTO(Comment Comment)
    {
        return new CommentResultDTO(
            id: Comment.Id,
            content: Comment.Content.Value,
            todoId: Comment.TodoId,
            createdDateTime: Comment.CreatedDateTime,
            updatedDateTime: Comment.UpdatedDateTime,
            ownerUserId: Comment.OwnerUserId
        );
    }
}
