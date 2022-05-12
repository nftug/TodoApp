namespace Domain
{
    public class TodoItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
    }

    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public DateTime? CreatedAt { get; set; }

        public string? Secret { get; set; } = string.Empty;

        public TodoItemDTO ItemToDTO() =>
            new TodoItemDTO
            {
                Id = Id,
                Name = Name,
                DueDate = DueDate,
                IsComplete = IsComplete,
                CreatedAt = CreatedAt
            };
    }
}