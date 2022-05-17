using Infrastructure.Shared;

namespace Infrastructure.Comments;

public class CommentQueryParameter : PaginationQueryParameterBase
{
    public string? q { get; set; }
    public string? Content { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
}
