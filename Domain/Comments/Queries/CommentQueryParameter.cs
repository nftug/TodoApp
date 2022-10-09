using Domain.Comments.Entities;
using Domain.Shared.Queries;

namespace Domain.Comments.Queries;

public class CommentQueryParameter : IQueryParameter<Comment>
{
    public string? Q { get; set; }
    public string? Content { get; set; }
    public string? UserName { get; set; }
    public Guid? UserId { get; set; }
    public int? Page { get; set; } = 1;
    public int? Limit { get; set; } = 10;
    public int? StartIndex { get; set; }
    public string Sort { get; set; } = "-UpdatedOn";
}
