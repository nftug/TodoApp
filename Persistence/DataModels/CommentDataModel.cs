namespace Persistence.DataModels;

public class CommentDataModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public TodoDataModel Todo { get; set; } = null!;
    public Guid TodoId { get; set; }
}
