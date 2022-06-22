using Domain.Comments;

namespace Infrastructure.DataModels;

public class CommentDataModel : DataModelBase<Comment>
{
    public string Content { get; set; } = string.Empty;
    public TodoDataModel Todo { get; set; } = null!;
    public Guid TodoId { get; set; }
}
