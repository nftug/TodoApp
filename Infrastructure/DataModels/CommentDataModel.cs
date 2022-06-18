namespace Infrastructure.DataModels;

public class CommentDataModel : DataModelBase
{
    public string Content { get; set; } = string.Empty;
    public TodoDataModel Todo { get; set; } = null!;
    public Guid TodoId { get; set; }
}
