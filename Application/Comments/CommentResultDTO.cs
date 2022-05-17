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

    public static CommentResultDTO CreateResultDTO(Comment comment)
    {
        return new CommentResultDTO(
            id: comment.Id,
            content: comment.Content.Value,
            todoId: comment.TodoId,
            createdDateTime: comment.CreatedDateTime,
            updatedDateTime: comment.UpdatedDateTime,
            ownerUserId: comment.OwnerUserId
        );
    }
}
