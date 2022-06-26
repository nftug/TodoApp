using Domain.Interfaces;

namespace Domain.Comment;

public class CommentQueryParameter : IQueryParameter<CommentModel>
{
    public string? Q { get; set; }
    public string? Content { get; set; }
    public string? UserName { get; set; }
    public Guid? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
    public string Sort { get; init; } = "-updatedOn";
}
