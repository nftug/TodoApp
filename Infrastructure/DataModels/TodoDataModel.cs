using Domain;

namespace Infrastructure.DataModels;

public class TodoDataModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? BeginDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }
    public int State { get; set; }
    public ICollection<CommentDataModel> Comments { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public UserDataModel? OwnerUser { get; set; }
    public string? OwnerUserId { get; set; }
}