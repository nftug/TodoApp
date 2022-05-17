namespace Application.Todos;

public class TodoCommandDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? BeginDateTime { get; set; }
    public DateTime? DueDateTime { get; set; }
    public int? State { get; set; }
}
