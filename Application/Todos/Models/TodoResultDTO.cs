using System.Text.Json.Serialization;
using Application.Comments.Models;
using Application.Shared.Interfaces;
using Domain.Todos.Entities;

namespace Application.Todos.Models;

public class TodoResultDTO : IResultDTO<Todo>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string State { get; init; } = null!;
    public List<CommentResultDTO> Comments { get; init; } = null!;
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
    public Guid? OwnerUserId { get; init; }

    [JsonConstructor]
    public TodoResultDTO() { }

    public TodoResultDTO(Todo todo)
    {
        Id = todo.Id;
        Title = todo.Title.Value;
        Description = todo.Description?.Value;
        StartDate = todo.Period?.StartDateValue;
        EndDate = todo.Period?.EndDateValue;
        State = todo.State.DisplayValue;
        Comments = todo.Comments
            .Select(x => new CommentResultDTO(x))
            .ToList();
        CreatedOn = todo.CreatedOn;
        UpdatedOn = todo.UpdatedOn;
        OwnerUserId = todo.OwnerUserId;
    }
}
