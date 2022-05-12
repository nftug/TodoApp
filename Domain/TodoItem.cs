using Domain.Interfaces;

namespace Domain
{
    public class TodoItem : IModel<TodoItemDTO>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
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
                Comments = Comments.Select(x => x.ItemToDTO()).ToList(),
                CreatedAt = CreatedAt
            };
    }

    public class TodoItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public DateTime? CreatedAt { get; set; }
    }
}