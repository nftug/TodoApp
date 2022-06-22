using Domain.Todo;

namespace Infrastructure.DataModels;

public class TodoDataModel : DataModelBase<TodoModel>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int State { get; set; }
    public ICollection<CommentDataModel> Comments { get; set; } = null!;
}