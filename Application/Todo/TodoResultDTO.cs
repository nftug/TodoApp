using Domain.Todo;
using Application.Comment;
using Application.Shared.Interfaces;

namespace Application.Todo;

public class TodoResultDTO : IResultDTO<TodoModel>
{
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public string? Description { get; }
    public DateTime? BeginDateTime { get; }
    public DateTime? DueDateTime { get; }
    public TodoState State { get; }
    public List<CommentResultDTO> Comments { get; } = null!;
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }
    public Guid? OwnerUserId { get; }

    public TodoResultDTO(TodoModel todo)
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
