using Application.Shared.Interfaces;
using Domain.Comments;

namespace Application.Comments;

public class CommentResultDTO : IResultDTO<Comment>
{
    public Guid Id { get; }
    public string Content { get; } = string.Empty;
    public Guid TodoId { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }
    public string? OwnerUserId { get; }

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
