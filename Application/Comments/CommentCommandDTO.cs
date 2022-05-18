namespace Application.Comments;

public class CommentCommandDTO
{
    public Guid Id { get; set; }
    public string? Content { get; set; } = null!;
    public Guid TodoId { get; set; }
}
