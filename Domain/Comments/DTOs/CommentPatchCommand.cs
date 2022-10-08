using Domain.Comments.Entities;
using Domain.Comments.ValueObjects;
using Domain.Shared.DTOs;

namespace Domain.Comments.DTOs;

public class CommentPatchCommand : ICommand<Comment>
{
    public Guid? Id { get; set; }
    [CommentContent(isPatch: true)]
    public string? Content { get; set; } = null!;

    public CommentPatchCommand() { }

    public CommentPatchCommand(CommentResultDTO origin)
    {
        Id = origin.Id;
        Content = origin.Content;
    }
}