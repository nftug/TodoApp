using Application.Shared.Interfaces;
using Domain.Comments;

namespace Application.Comments;

public class CommentCommandDTO : ICommandDTO<Comment>
{
    public Guid Id { get; init; }
    [CommentContent]
    public string? Content { get; init; } = null!;
    public Guid TodoId { get; init; }
}
