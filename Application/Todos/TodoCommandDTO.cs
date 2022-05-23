using Domain.Todos;

namespace Application.Todos;

public class TodoCommandDTO
{
    public Guid Id { get; set; }
    [TodoTitleAttribute]
    public string? Title { get; set; }
    [TodoDescriptionAttribute]
    public string? Description { get; set; }
    [TodoPeriodAttribute(Period.Begin, "DueDateTime")]
    public DateTime? BeginDateTime { get; set; }
    [TodoPeriodAttribute(Period.Due, "BeginDateTime")]
    public DateTime? DueDateTime { get; set; }
    [TodoStateAttribute]
    public int? State { get; set; }
}
