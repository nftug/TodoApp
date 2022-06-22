using Domain.Todo;
using Application.Comment;
using Application.Shared.Interfaces;

namespace Application.Todo;

public class TodoResultDTO : IResultDTO<TodoModel>
{
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public string? Description { get; }
    public DateTime? StartDate { get; }
    public DateTime? EndDate { get; }
    public TodoState State { get; }
    public List<CommentResultDTO> Comments { get; } = null!;
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; }
    public Guid? OwnerUserId { get; }

    public TodoResultDTO(TodoModel todo)
    {
        Id = todo.Id;
        Title = todo.Title.Value;
        Description = todo.Description?.Value;
        StartDate = todo.Period?.StartDateValue;
        EndDate = todo.Period?.EndDateValue;
        State = todo.State;
        Comments = todo.Comments
            .Select(x => new CommentResultDTO(x))
            .ToList();
        CreatedOn = todo.CreatedOn;
        UpdatedOn = todo.UpdatedOn;
        OwnerUserId = todo.OwnerUserId;
    }
}
