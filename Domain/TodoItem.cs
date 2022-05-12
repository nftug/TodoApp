using Domain.Interfaces;

namespace Domain
{
    public class TodoItem : IModel<TodoItemDTO>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public List<Comment>? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Secret Fields
        public string? Secret { get; set; } = string.Empty;

        public TodoItemDTO ItemToDTO()
            => new TodoItemDTO
            {
                Id = Id,
                Name = Name,
                DueDateTime = DueDateTime,
                IsComplete = IsComplete,
                Comments = Comments,
                CreatedAt = CreatedAt
            };
    }

    public class TodoItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public List<Comment>? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}