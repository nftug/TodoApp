namespace Infrastructure.DataModels;

public class CommentDataModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public TodoDataModel Todo { get; set; } = null!;
    public Guid TodoId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public UserDataModel? OwnerUser { get; set; }
    public string? OwnerUserId { get; set; }
}
