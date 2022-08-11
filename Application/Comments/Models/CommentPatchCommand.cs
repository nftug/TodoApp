using Application.Shared.Interfaces;
using Domain.Comments.Entities;
using Domain.Comments.ValueObjects;

namespace Application.Comments.Models;

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