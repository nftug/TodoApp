using Domain.Todos.Entities;

namespace Infrastructure.DataModels;

public class TodoDataModel : DataModelBase<Todo>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int State { get; set; }
    public IList<CommentDataModel> Comments { get; set; } = new List<CommentDataModel>();

    public TodoDataModel() { }

    public TodoDataModel(Todo origin)
    {
        Transfer(origin);
    }

    public void Transfer(Todo origin)
    {
        base.Transfer(origin);

        Title = origin.Title.Value;
        Description = origin.Description.Value;
        StartDate = origin.Period.StartDateValue;
        EndDate = origin.Period.EndDateValue;
        State = origin.State.Value;
    }
}