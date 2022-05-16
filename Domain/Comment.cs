namespace Domain;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public TodoItem? TodoItem { get; set; }
    public Guid TodoItemId { get; set; }
}
