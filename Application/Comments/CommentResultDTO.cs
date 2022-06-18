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

    public CommentResultDTO(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content.Value;
        TodoId = comment.TodoId;
        CreatedDateTime = comment.CreatedDateTime;
        UpdatedDateTime = comment.UpdatedDateTime;
        OwnerUserId = comment.OwnerUserId;
    }
}
