using Domain.Comments.Entities;

namespace Infrastructure.DataModels;

public class CommentDataModel : DataModelBase<Comment>
{
    public string Content { get; set; } = string.Empty;
    public TodoDataModel Todo { get; set; } = null!;
    public Guid TodoId { get; set; }

    public CommentDataModel() { }

    public CommentDataModel(Comment origin)
    {
        Transfer(origin);
    }

    public void Transfer(Comment origin)
    {
        base.Transfer(origin);

        Content = origin.Content.Value;
        TodoId = origin.TodoId;
    }
}
