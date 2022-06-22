using Application.Shared.Interfaces;
using Domain.Comment;

namespace Application.Comment;

public class CommentCommandDTO : ICommandDTO<CommentModel>
{
    public Guid? Id { get; init; }
    [CommentContent]
    public string? Content { get; init; } = null!;
    public Guid TodoId { get; init; }
}
