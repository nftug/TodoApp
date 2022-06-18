using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Infrastructure.Comments;

public class CommentQueryParameter : IQueryParameter<CommentDataModel>
{
    public string? q { get; set; }
    public string? Content { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}
