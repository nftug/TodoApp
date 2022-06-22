using Domain.Comment;
using Domain.Interfaces;

namespace Infrastructure.Comment;

public class CommentQueryParameter : IQueryParameter<CommentModel>
{
    public string? Q { get; init; }
    public string? Content { get; init; }
    public string? UserName { get; init; }
    public Guid? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
