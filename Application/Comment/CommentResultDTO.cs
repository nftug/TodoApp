using Application.Shared.Interfaces;
using Domain.Comment;

namespace Application.Comment;

public class CommentResultDTO : IResultDTO<CommentModel>
{
    public Guid Id { get; }
    public string Content { get; } = string.Empty;
    public Guid TodoId { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }
    public Guid? OwnerUserId { get; }

    public CommentResultDTO(CommentModel comment)
    {
        Id = comment.Id;
        Content = comment.Content.Value;
        TodoId = comment.TodoId;
        CreatedDateTime = comment.CreatedDateTime;
        UpdatedDateTime = comment.UpdatedDateTime;
        OwnerUserId = comment.OwnerUserId;
    }
}
