namespace Application.Comments
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public Guid TodoItemId { get; set; }
    }
}