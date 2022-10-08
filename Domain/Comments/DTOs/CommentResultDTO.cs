using System.Text.Json.Serialization;
using Domain.Comments.Entities;
using Domain.Shared.DTOs;

namespace Domain.Comments.DTOs;

public class CommentResultDTO : IResultDTO<Comment>
{
    public Guid Id { get; init; }
    public string Content { get; init; } = string.Empty;
    public Guid TodoId { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
    public Guid? OwnerUserId { get; init; }

    [JsonConstructor]
    public CommentResultDTO() { }

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
