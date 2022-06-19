using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Infrastructure.Comments;

public class CommentQueryParameter : IQueryParameter<CommentDataModel>
{
    public string? Q { get; init; }
    public string? Content { get; init; }
    public string? UserName { get; init; }
    public string? UserId { get; set; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}
