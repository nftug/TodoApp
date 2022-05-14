using Application.Comments;

namespace Application.TodoItems
{
    public class TodoItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; } = null;
        public bool? IsComplete { get; set; } = false;
        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public DateTime? CreatedAt { get; set; }
        public string? CreatedById { get; set; }
    }
}