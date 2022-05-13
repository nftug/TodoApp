using Domain.Interfaces;

namespace Domain
{
    public class Comment : IModel<CommentDTO>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public TodoItem? TodoItem { get; set; }
        public Guid TodoItemId { get; set; }

        public CommentDTO ToDTO()
            => new CommentDTO
            {
                Id = Id,
                Content = Content,
                CreatedAt = CreatedAt,
                TodoItemId = TodoItemId
            };

    }

    public class CommentDTO : IDTOModel<Comment>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public Guid TodoItemId { get; set; }

        public Comment ToRawModel()
            => new Comment
            {
                Id = Id,
                Content = Content,
                CreatedAt = CreatedAt,
                TodoItemId = TodoItemId
            };
    }
}