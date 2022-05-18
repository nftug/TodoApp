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
    public int State { get; set; }
    public ICollection<CommentResultDTO> Comments { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string? OwnerUserId { get; set; }

    public TodoResultDTO(
        Guid id,
        string title,
        string? description,
        DateTime? beginDateTime,
        DateTime? dueDateTime,
        int state,
        ICollection<CommentResultDTO> comments,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        Id = id;
        Title = title;
        Description = description;
        BeginDateTime = beginDateTime;
        DueDateTime = dueDateTime;
        State = state;
        Comments = comments;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        OwnerUserId = ownerUserId;
    }

    public static TodoResultDTO CreateResultDTO(Todo todo)
    {
        var comments = todo.Comments.Select(x =>
            CommentResultDTO.CreateResultDTO(x)
        ).ToList();

        return new TodoResultDTO(
            id: todo.Id,
            title: todo.Title.Value,
            description: todo.Description?.Value,
            beginDateTime: todo.Period?.BeginDateTimeValue,
            dueDateTime: todo.Period?.DueDateTimeValue,
            state: todo.State.Value,
            comments: comments,
            createdDateTime: todo.CreatedDateTime,
            updatedDateTime: todo.UpdatedDateTime,
            ownerUserId: todo.OwnerUserId
        );
    }
}
