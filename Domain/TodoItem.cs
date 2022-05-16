namespace Domain;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? DueDateTime { get; set; } = null;
    public bool? IsComplete { get; set; } = false;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public DateTime? CreatedAt { get; set; }
    public ApplicationUser? CreatedBy { get; set; }
    public string? CreatedById { get; set; }

    // Secret Fields
    public string? Secret { get; set; } = string.Empty;
}
