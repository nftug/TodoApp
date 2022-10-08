using Domain.Comments.Entities;
using Domain.Comments.ValueObjects;
using Domain.Shared.DTOs;

namespace Domain.Comments.DTOs;

public class CommentCommand : ICommand<Comment>
{
    public Guid? Id { get; set; }
    [CommentContent]
    public string? Content { get; set; } = null!;
    public Guid TodoId { get; set; }

    public CommentCommand() { }

    public CommentCommand(CommentResultDTO origin)
    {
        Id = origin.Id;
        Content = origin.Content;
        TodoId = origin.TodoId;
    }
}
