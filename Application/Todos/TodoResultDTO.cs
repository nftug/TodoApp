using Domain.Todos;
using Application.Comments;

namespace Application.Todos;

public class TodoResultDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? BeginDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }
    public TodoState State { get; set; }
    public List<CommentResultDTO> Comments { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string? OwnerUserId { get; set; }

    public TodoResultDTO(Todo todo)
    {
        Id = todo.Id;
        Title = todo.Title.Value;
        Description = todo.Description?.Value;
        BeginDateTime = todo.Period?.BeginDateTimeValue;
        DueDateTime = todo.Period?.DueDateTimeValue;
        State = todo.State;
        Comments = todo.Comments
            .Select(x => new CommentResultDTO(x))
            .ToList();
        CreatedDateTime = todo.CreatedDateTime;
        UpdatedDateTime = todo.UpdatedDateTime;
        OwnerUserId = todo.OwnerUserId;
    }
}
