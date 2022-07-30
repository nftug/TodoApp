using Application.Shared.Interfaces;
using Domain.Comments.Entities;

namespace Application.Comments.Models;

public class CommentResultDTO : IResultDTO<Comment>
{
    public Guid Id { get; }
    public string Content { get; } = string.Empty;
    public Guid TodoId { get; }
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; }
    public Guid? OwnerUserId { get; }

    public CommentResultDTO(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content.Value;
        TodoId = comment.TodoId;
        CreatedOn = comment.CreatedOn;
        UpdatedOn = comment.UpdatedOn;
        OwnerUserId = comment.OwnerUserId;
    }
}
