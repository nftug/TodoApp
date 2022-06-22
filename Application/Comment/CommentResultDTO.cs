using Application.Shared.Interfaces;
using Domain.Comment;

namespace Application.Comment;

public class CommentResultDTO : IResultDTO<CommentModel>
{
    public Guid Id { get; }
    public string Content { get; } = string.Empty;
    public Guid TodoId { get; }
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; }
    public Guid? OwnerUserId { get; }

    public CommentResultDTO(CommentModel comment)
    {
        Id = comment.Id;
        Content = comment.Content.Value;
        TodoId = comment.TodoId;
        CreatedOn = comment.CreatedOn;
        UpdatedOn = comment.UpdatedOn;
        OwnerUserId = comment.OwnerUserId;
    }
}
