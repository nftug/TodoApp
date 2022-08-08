using Application.Shared.Interfaces;
using Domain.Comments.Entities;
using Domain.Comments.ValueObjects;

namespace Application.Comments.Models;

public class CommentCommandDTO : ICommandDTO<Comment>
{
    public Guid? Id { get; set; }
    [CommentContent]
    public string? Content { get; set; } = null!;
    public Guid TodoId { get; set; }
}
